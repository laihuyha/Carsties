/* eslint-disable no-undef */
import { getTokenWorkaround } from "@/app/_actions/auth-actions";
import { FetchResult } from "@/types";

const baseUrl = process.env.GATE_WAY_SERVICE_URI;

let fetchOptions: RequestInit = {
  // cache: "force-cache" as RequestCache, // Convert the string to RequestCache
  next: { revalidate: 90 },
};

const handleResponse = async <T>(response: any) => {
  const text = await response.text();
  // const data = text && JSON.parse(text);
  let data;
  try {
    data = JSON.parse(text);
  } catch (error) {
    data = text;
  }

  if (response.ok) {
    return {
      data: data || response.statusText,
    } as FetchResult<T>;
  } else {
    const error = {
      status: response.status,
      message:
        typeof data === "string" && data.length > 0
          ? data
          : typeof data === "object" && data.error !== null
          ? data.error
          : response.statusText,
    };
    return handleError(error);
  }
};

const handleError = (error: any) => {
  return {
    data: null,
    error: error,
  } as FetchResult<any>;
};

const getToken = async () => {
  try {
    const session = await getTokenWorkaround();
    // return undefined;
    return session?.accessToken;
  } catch (error) {
    console.error("Error getting token:", error);
    return undefined;
  }
};

export const agent = {
  get: async <T>(url: string, tag?: string[]): Promise<FetchResult<T>> => {
    fetchOptions = {
      ...fetchOptions,
      next: { ...fetchOptions.next, tags: tag },
    };
    const res = await fetch(baseUrl + url, {
      headers: {
        Authorization: `Bearer ${await getToken()}`,
      },
      ...fetchOptions,
    });
    return handleResponse(res);
  },

  post: async <T>(url: string, body: {}): Promise<FetchResult<T>> => {
    const token = await getToken();
    const headers = { "Content-type": "application/json" } as any;
    if (token) {
      headers.Authorization = `Bearer ${token}`;
    }

    const res = await fetch(baseUrl + url, {
      method: "POST",
      headers,
      ...fetchOptions,
      body: JSON.stringify(body),
    });

    return handleResponse(res);
  },

  put: async <T>(url: string, body: {}): Promise<FetchResult<T>> => {
    const token = await getToken();
    const headers = { "Content-type": "application/json" } as any;
    if (token) {
      headers.Authorization = `Bearer ${token}`;
    }

    const res = await fetch(baseUrl + url, {
      method: "PUT",
      headers,
      ...fetchOptions,
      body: JSON.stringify(body),
    });

    return handleResponse(res);
  },

  del: async <T>(url: string): Promise<FetchResult<T>> => {
    const token = await getToken();
    const headers = { "Content-type": "application/json" } as any;
    if (token) {
      headers.Authorization = `Bearer ${token}`;
    }

    const res = await fetch(baseUrl + url, {
      method: "DELETE",
      headers,
      ...fetchOptions,
    });

    return handleResponse(res);
  },
};
