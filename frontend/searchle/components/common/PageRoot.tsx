import styles from "./PageRoot.module.scss";
import { PropsWithChildren, useState } from "react";
import { styled } from "@nextui-org/react";
import React from "react";

interface Props extends PropsWithChildren {

}

const PageRootElement = styled("div", {});

const PageRoot = (props: Props) => {
  const [windowHeight, setWindowHeight] = useState<number>(0);
  React.useEffect(() => {
    setWindowHeight(window.innerHeight);
    function handleResize() {
      setWindowHeight(window.innerHeight);
    }

    window.addEventListener('resize', handleResize)
  })

  return (
    <PageRootElement
      className={styles.root}
      style={{
        height: windowHeight > 0 ? windowHeight + "px" : "100vh"
      }}
    >
      {props.children}
    </PageRootElement>
  )
}

export default PageRoot;