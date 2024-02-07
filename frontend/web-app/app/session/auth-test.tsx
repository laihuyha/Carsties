"use client";

import { Button } from "flowbite-react";
import { useState } from "react";
import { update } from "../actions/auction-action";

export const AuthTest = () => {
  const [loading, setLoading] = useState(false);
  const [result, setResult] = useState<any>();

  const doUpdate = () => {
    setResult(undefined);
    setLoading(true);
    const res = update("afbee524-5972-4075-8800-7d1f9d7b0a0c")
      .then((res) => setResult(res))
      .finally(() => setLoading(false));
  };
  return (
    <div className="flex items-center gap-4">
      <Button
        outline
        isProcessing={loading}
        onClick={doUpdate}
        children="TEst Auth"
      />
      <div>{result}</div>
    </div>
  );
};
