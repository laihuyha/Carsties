import NextAuth from "next-auth";
import authConfig from "@/auth.config";
import { DEFAUTLT_LOGGED_IN_REDIRECT, publicRoutes } from "@/routes";

export const { auth } = NextAuth(authConfig);

export default auth((req) => {
  const isLoggedIn = !!req.auth;
  const isAuthRoute = req.nextUrl.pathname.startsWith("/api/auth");
  const isPublishRoutes = publicRoutes.includes(req.nextUrl.pathname);
  if (isAuthRoute) {
    if (isLoggedIn)
      return Response.redirect(new URL(DEFAUTLT_LOGGED_IN_REDIRECT, req.url));
    return null;
  }
  if (!isLoggedIn && !isPublishRoutes) {
    console.log("ERROR: Prohibited route caused by not authenticated! ");
    // return Response.redirect(new URL("", req.url));
  }
});

// Optionally, don't invoke Middleware on some paths
// See "Matching Paths" below to learn more
export const config = {
  matcher: ["/((?!.+\\.[\\w]+$|_next).*)", "/", "/(api|trpc)(.*)"],
};
