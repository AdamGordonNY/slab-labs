import { createAuthClient } from "better-auth/react";

export const client = createAuthClient({
  baseURL: process.env.NEXT_PUBLIC_API_URL,
});

export const { signUp, signIn, signOut, useSession } = client;
