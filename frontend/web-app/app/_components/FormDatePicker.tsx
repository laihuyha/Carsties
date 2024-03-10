"use client";

import { Button } from "@/components/ui/button";
import { Calendar } from "@/components/ui/calendar";
import {
  FormControl,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
} from "@/components/ui/form";
import { cn } from "@/lib/utils";
import { CalendarIcon } from "@radix-ui/react-icons";
import {
  Popover,
  PopoverContent,
  PopoverTrigger,
} from "@radix-ui/react-popover";
import { format } from "date-fns";
import { UseFormReturn } from "react-hook-form";

type Props = {
  formContext: UseFormReturn<any, any, any>;
  name: string;
  label?: string;
  showLabel: boolean;
};

const FormDatePicker = ({ formContext, name, label, showLabel }: Props) => {
  return (
    <>
      <FormField
        control={formContext.control}
        name={name}
        render={({ field }) => (
          <FormItem className="flex flex-col mt-2">
            {showLabel ? <FormLabel>{label ?? field.name}</FormLabel> : null}
            <Popover>
              <PopoverTrigger asChild>
                <FormControl>
                  <Button
                    variant={"outline"}
                    className={cn(
                      "w-auto pl-3 text-left font-normal",
                      !field.value && "text-muted-foreground"
                    )}
                  >
                    {field.value ? (
                      format(field.value, "PPP")
                    ) : (
                      <span>Pick a date</span>
                    )}
                    <CalendarIcon className="ml-auto h-4 w-4 opacity-50" />
                  </Button>
                </FormControl>
              </PopoverTrigger>
              <PopoverContent className="w-auto p-0" align="start">
                <Calendar
                  className="bg-white"
                  mode="single"
                  selected={field.value}
                  onSelect={field.onChange}
                  disabled={(date) => date < new Date()}
                  initialFocus
                />
              </PopoverContent>
            </Popover>
            <FormMessage />
          </FormItem>
        )}
      />
    </>
  );
};

export default FormDatePicker;
