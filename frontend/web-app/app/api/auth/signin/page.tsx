"use client";

import Empty from "@/app/_components/Empty";

const SignInPage = ({
  searchParams,
}: {
  searchParams: { callbackUrl: string };
}) => {
  return (
    <Empty
      title="You need to login to continue"
      subtitle="Click the button below"
      showLogin
      callbackUrl={searchParams.callbackUrl}
    />
  );
};

export default SignInPage;
