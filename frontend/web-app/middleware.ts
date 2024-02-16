//#region Advanced
import { getToken } from "next-auth/jwt";
import { NextRequest, NextResponse } from "next/server";
import { apiAuthRoutePrefix, authRoutes, publicRoutes } from "./routes";

// This function can be marked `async` if using `await` inside
export async function middleware(request: NextRequest) {
  const { nextUrl } = request;

  const callbackUrl = encodeURI(nextUrl.pathname);

  /**
   * This is the alternative way to get session from server
   * @params {NextRequest} request
   * @params {string} secret
   */
  const session = await getToken({
    req: request,
    secret: process.env.NEXTAUTH_SECRET,
  });

  const isLoggedIn = (session !== null && session !== undefined) || false;
  const isPublicRoute = publicRoutes.includes(nextUrl.pathname);
  const isAuthRequiredRoute = authRoutes.includes(nextUrl.pathname);
  const isApiAuthRoute = nextUrl.pathname.startsWith(apiAuthRoutePrefix);

  if (isApiAuthRoute) return null;

  /**
   * If user is logged in, set Authorization header
   * This token can be set in agent.ts too
   * TODO : Add Authorization and set Bearer token in here instead of using in agent
   */
  // if (isLoggedIn) {
  //   // request.headers.set("Authorization", `Bearer ${session?.access_token}`);
  // }

  if (!isLoggedIn && !isPublicRoute) {
    return NextResponse.redirect(
      new URL(
        `/api/auth/signin/?${new URLSearchParams({
          callbackUrl,
        })}`,
        request.url
      )
    );
  }
  if (isAuthRequiredRoute) {
    if (isLoggedIn) {
      return null;
    } else {
      return NextResponse.redirect(
        new URL(
          `/api/auth/signin/?${new URLSearchParams({
            callbackUrl,
          })}`,
          request.url
        )
      );
    }
  }
}

// // See "Matching Paths" below to learn more
export const config = {
  matcher: ["/((?!.+\\.[\\w]+$|_next).*)", "/", "/(api|trpc)(.*)"],
};

//#endregion

//#region Basic
// export { default } from "next-auth/middleware";
// export const config = {
//   matcher: ["/session"],
//   pages: {
//     signIn: "/api/auth/signin",
//   },
// };
//#endregion
