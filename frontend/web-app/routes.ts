/**
 * Array of routes accessible without authentication
 * @type {string[]}
 */
export const publicRoutes: string[] = ["/"];

/**
 * Array of routes accessible with authentication
 * These routes will redirect to the login page first if not authenticated
 * @type {string[]}
 */
export const authRoutes: string[] = [];

/**
 * The prefix of the API routes
 * Routes will be prefixed with this for authentication purposes
 * @type {string}
 */
export const apiRoutePrefix = "/api/auth";

export const DEFAUTLT_LOGGED_IN_REDIRECT = "/";
