import { Button, ButtonProps, NormalColors } from "@nextui-org/react";
import { PropsWithChildren, useState } from "react";
import ConfirmationDialog from "./ConfirmationDialog";

interface Props extends ButtonProps {
  onConfirmed(): void;
  dialogCancelText?: string;
  dialogConfirmText?: string;
  dialogContents?: JSX.Element;
  dialogConfirmBtnColor?: NormalColors;
  dialogCancelBtnColor?: NormalColors;
}
const ConfirmationButton = (
  {
    onConfirmed,
    dialogCancelText,
    dialogConfirmText,
    dialogContents,
    dialogConfirmBtnColor,
    dialogCancelBtnColor,
    children,
    ...props
  }: Props) => {
  const [open, setOpen] = useState<boolean>(false);

  const dialogClose = () => {
    setOpen(false);
  }

  const buttonClicked = () => {
    setOpen(true);
  }

  const dialogConfirm = () => {
    setOpen(false);
    onConfirmed();
  }

  return (
    <>
      <Button {...props} onClick={buttonClicked}>
        {children}
      </Button>
      <ConfirmationDialog
        confirmText={dialogConfirmText}
        cancelText={dialogCancelText}
        onClose={dialogClose}
        onConfirm={dialogConfirm}
        open={open}
        confirmColor={dialogConfirmBtnColor}
        cancelColor={dialogCancelBtnColor}
      >
        {dialogContents}
      </ConfirmationDialog>
    </>
  );
}

export default ConfirmationButton;