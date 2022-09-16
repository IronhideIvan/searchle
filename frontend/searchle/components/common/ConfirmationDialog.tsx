import { Button, Modal, NormalColors } from "@nextui-org/react";
import { PropsWithChildren } from "react";

interface Props extends PropsWithChildren {
  open: boolean;
  onConfirm(): void;
  onClose(): void;
  confirmText?: string;
  cancelText?: string;
  confirmColor?: NormalColors;
  cancelColor?: NormalColors;
}
const ConfirmationDialog = (props: Props) => {

  const closeModal = () => {
    if (props.onClose) {
      props.onClose();
    }
  }

  const confirmClicked = () => {
    if (props.onConfirm) {
      props.onConfirm();
    }
  }

  return (
    <Modal
      open={props.open}
      aria-label="Confirmation dialog"
      onClose={closeModal}
    >
      <Modal.Body>
        {props.children}
      </Modal.Body>
      <Modal.Footer>
        <Button
          auto
          flat
          color={props.cancelColor ? props.cancelColor : "secondary"}
          onClick={closeModal}
        >
          {props.cancelText ? props.cancelText : "Cancel"}
        </Button>
        <Button
          auto
          color={props.confirmColor ? props.confirmColor : "default"}
          onClick={confirmClicked}
        >
          {props.confirmText ? props.confirmText : "Confirm"}
        </Button>
      </Modal.Footer>
    </Modal>
  );
}

export default ConfirmationDialog;