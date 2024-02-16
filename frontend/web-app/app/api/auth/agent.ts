import { getTokenWorkaround } from "@/app/actions/auth-actions";

const baseUrl = process.env.GATE_WAY_SERVICE_URI;

const fetchOptions: RequestInit = {
  cache: "force-cache" as RequestCache, // Convert the string to RequestCache
};

const handleResponse = async (response: Response) => {
  const text = await response.text();
  // const data = text && JSON.parse(text);
  let data;
  try {
    data = JSON.parse(text);
  } catch (error) {
    data = text;
  }

  if (response.ok) {
    return data || response.statusText;
  } else {
    const error = {
      status: response.status,
      message:
        typeof data === "string" && data.length > 0
          ? data
          : response.statusText,
    };
    return { error };
  }
};

const handleError = (error: any) => {
  return { error };
};

const getToken = async () => {
  try {
    const session = await getTokenWorkaround();
    // return undefined;
    return session?.access_token;
  } catch (error) {
    console.error("Error getting token:", error);
    return undefined;
  }
};

export const agent = {
  get: async <T>(url: string): Promise<T> =>
    fetch(baseUrl + url, {
      headers: {
        Authorization: `Bearer ${await getToken()}`,
      },
      ...fetchOptions,
    })
      .then(handleResponse)
      .catch(handleError),

  post: async <T>(url: string, body: {}): Promise<T> => {
    const token = await getToken();
    const headers = { "Content-type": "application/json" } as any;
    if (token) {
      headers.Authorization = `Bearer ${token}`;
    }

    return fetch(baseUrl + url, {
      method: "POST",
      headers,
      ...fetchOptions,
      body: JSON.stringify(body),
    })
      .then(handleResponse)
      .catch(handleError);
  },

  put: async <T>(url: string, body: {}): Promise<T> => {
    const token = await getToken();
    const headers = { "Content-type": "application/json" } as any;
    if (token) {
      headers.Authorization = `Bearer ${token}`;
    }

    return fetch(baseUrl + url, {
      method: "PUT",
      headers,
      ...fetchOptions,
      body: JSON.stringify(body),
    })
      .then(handleResponse)
      .catch(handleError);
  },

  del: async <T>(url: string): Promise<T> =>
    fetch(baseUrl + url, {
      method: "DELETE",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${await getToken()}`,
      },
      ...fetchOptions,
    })
      .then(handleResponse)
      .catch(handleError),
};
