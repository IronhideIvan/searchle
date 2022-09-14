import { Modal, ModalProps } from "@nextui-org/react";
import React from "react";
import { useState } from "react";

interface Props extends ModalProps {
}

const ResponsiveModal = ({ children, ...props }: Props) => {
  const [screenWidth, setScreenWidth] = useState<number>(0);
  React.useEffect(() => {
    setScreenWidth(window.innerWidth);
  })

  return (
    <Modal
      {...props}
      fullScreen={screenWidth < 768}
    >
      {children}
    </Modal>
  );
}

export default ResponsiveModal;