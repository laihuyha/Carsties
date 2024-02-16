import { createWithEqualityFn } from "zustand/traditional";

type State = {
  loading: boolean;
};

type Action = {
  setLoading: (loading: boolean) => void;
  setParamsValue: <K extends keyof State>(key: K, newValue: State[K]) => void;
};

const initialState: State = {
  loading: false,
};

export const useCommonStore = createWithEqualityFn<State & Action>()((set) => ({
  ...initialState,
  setLoading: (loading: boolean) => set((state) => ({ ...state, loading })),
  setParamsValue: (value: keyof State, newValue: any) =>
    set((state) => ({ ...state, [value]: newValue })),
}));
