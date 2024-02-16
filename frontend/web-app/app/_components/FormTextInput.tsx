import {
  FormControl,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
} from "@/components/ui/form";
import { Input } from "@/components/ui/input";
import React from "react";
import {
  ControllerFieldState,
  ControllerRenderProps,
  UseFormReturn,
  UseFormStateReturn,
} from "react-hook-form";

type Props = {
  formContext: UseFormReturn<any, any, any>;
  name: string;
  type?: "text" | "number" | "password" | "email" | "tel" | "url" | "date";
  formItemClassName?: string;
  label?: string;
  showLabel?: boolean;
  placeholder?: string;
  render?: ({
    field,
    fieldState,
    formState,
  }: {
    field: ControllerRenderProps<any, string>;
    fieldState: ControllerFieldState;
    formState: UseFormStateReturn<any>;
  }) => React.ReactElement;
};

export const FormTextInput = (props: Props) => {
  return (
    <>
      <FormField
        control={props.formContext.control}
        name={props.name.toString()}
        render={
          props.render
            ? props.render
            : ({ field, fieldState }) => (
                <FormItem
                  className={
                    props.formItemClassName ? props.formItemClassName : "mb-3"
                  }
                  color={
                    props.formContext.formState.errors?.[props.name]
                      ? "failure"
                      : !fieldState.isDirty
                      ? "neutral"
                      : "success"
                  }
                >
                  {props.showLabel && props.label ? (
                    <FormLabel>{props.label}</FormLabel>
                  ) : props.showLabel && !props.label ? (
                    <FormLabel className="capitalize">{field.name}</FormLabel>
                  ) : null}
                  <FormControl>
                    <Input
                      placeholder={props.placeholder ? props.placeholder : ""}
                      type={props.type ? props.type : "text"}
                      {...field}
                    />
                  </FormControl>
                  <FormMessage />
                </FormItem>
              )
        }
      />
    </>
  );
};
