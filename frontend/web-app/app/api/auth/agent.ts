import { getTokenWorkaround } from "@/app/actions/auth-actions";

export const identityUrl = process.env.IDENTITY_SERVICE_URL;

const baseUrl = process.env.GATE_WAY_SERVICE_URI;

const handleResponse = (response: Response) => {
  if (!response.ok) {
    handleError(response.statusText);
  }
  return response.json();
};

const handleError = (error: any) => {
  if (error instanceof Response) {
    // Check if the response has a body
    const contentType = error.headers.get("content-type");
    if (contentType && contentType.includes("application/json")) {
      // Attempt to parse the JSON body
      return error
        .json()
        .then((parsedError) => {
          throw new Error(parsedError.message || error.statusText);
        })
        .catch(() => {
          // If parsing fails, throw an error with the status text
          throw new Error(error.statusText);
        });
    } else {
      // If there's no JSON body, throw an error with the status text
      throw new Error(error.statusText);
    }
  } else {
    // If the error is not a Response object, throw an error with the error itself
    console.log(JSON.stringify(error));
  }
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
    })
      .then(handleResponse)
      .catch(handleError),

  post: async <T>(url: string, body: {}): Promise<T> =>
    fetch(baseUrl + url, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${await getToken()}`,
      },
      body: JSON.stringify(body),
    })
      .then(handleResponse)
      .catch(handleError),

  put: async <T>(url: string, body: {}): Promise<T> =>
    fetch(baseUrl + url, {
      method: "PUT",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${await getToken()}`,
      },
      body: JSON.stringify(body),
    })
      .then(handleResponse)
      .catch(handleError),

  del: async <T>(url: string): Promise<T> =>
    fetch(baseUrl + url, {
      method: "DELETE",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${await getToken()}`,
      },
    })
      .then(handleResponse)
      .catch(handleError),
};
