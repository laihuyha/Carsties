const baseUrl = process.env.GATE_WAY_SERVICE_URI;

export const identityUrl = process.env.IDENTITY_SERVICE_URL;

const handleResponse = (response: Response) => {
  if (!response.ok) {
    throw new Error(`HTTP error! Status: ${response.status}`);
  }
  return response.json();
};

export const agent = {
  get: <T>(url: string): Promise<T> =>
    fetch(baseUrl + url)
      .then(handleResponse)
      .catch((error) => {
        throw new Error(error);
      }),

  post: <T>(url: string, body: {}): Promise<T> =>
    fetch(baseUrl + url, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(body),
    })
      .then(handleResponse)
      .catch((error) => {
        throw new Error(error);
      }),

  put: <T>(url: string, body: {}): Promise<T> =>
    fetch(baseUrl + url, {
      method: "PUT",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(body),
    })
      .then(handleResponse)
      .catch((error) => {
        throw new Error(error);
      }),

  del: <T>(url: string): Promise<T> =>
    fetch(baseUrl + url, { method: "DELETE" })
      .then(handleResponse)
      .catch((error) => {
        throw new Error(error);
      }),
};
