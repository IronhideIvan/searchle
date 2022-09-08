import { Button, ButtonProps, Loading } from "@nextui-org/react";

interface LoaderButtonProps extends ButtonProps {
  isLoading: boolean;
}

const LoaderButton = (props: LoaderButtonProps) => {
  return (
    <Button
      {...props}
      disabled={props.isLoading}
    >
      {props.isLoading
        ? <Loading color="currentColor" size="sm" />
        : props.children}
    </Button>
  );
}

export default LoaderButton;