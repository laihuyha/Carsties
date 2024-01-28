import DuendeIdentityServer6 from "next-auth/providers/duende-identity-server6";
import Google from "next-auth/providers/google";

import type { NextAuthConfig } from "next-auth";

export default {
  providers: [
    Google({
      clientId: process.env.GOOGLE_CLIENT_ID,
      clientSecret: process.env.GOOGLE_CLIENT_SECRET,
    }),
    DuendeIdentityServer6({
      id: "id-server",
      clientId: process.env.IDENTITY_SERVER_CLIENT_ID,
      clientSecret: process.env.IDENTITY_SERVER_CLIENT_SECRET,
      issuer: process.env.IDENTITY_SERVICE_URL,
      authorization: {
        params: {
          scope: "openid profile auctionApp",
        },
      },
    }),
  ],
} satisfies NextAuthConfig;
