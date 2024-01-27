const baseUrl = process.env.GATE_WAY_SERVICE_URI;

export const identityUrl = process.env.IDENTITY_SERVICE_URL;

const handleResponse = (response: Response) => {
  if (!response.ok) {
    handleError(response.statusText);
  }
  return response.json();
};

const handleError = (error: any) => {
  throw new Error(error);
};

export const agent = {
  get: <T>(url: string): Promise<T> =>
    fetch(baseUrl + url)
      .then(handleResponse)
      .catch(handleError),

  post: <T>(url: string, body: {}): Promise<T> =>
    fetch(baseUrl + url, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(body),
    })
      .then(handleResponse)
      .catch(handleError),

  put: <T>(url: string, body: {}): Promise<T> =>
    fetch(baseUrl + url, {
      method: "PUT",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(body),
    })
      .then(handleResponse)
      .catch(handleError),

  del: <T>(url: string): Promise<T> =>
    fetch(baseUrl + url, { method: "DELETE" })
      .then(handleResponse)
      .catch(handleError),
};
