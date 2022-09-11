import styles from "./PageRoot.module.scss";
import { PropsWithChildren, useState } from "react";
import { styled } from "@nextui-org/react";
import React from "react";

interface Props extends PropsWithChildren {

}

const PageRootElement = styled("div", {});
let resizeListenerSet: boolean = false;

const PageRoot = (props: Props) => {
  const [windowHeight, setWindowHeight] = useState<number>(0);
  React.useEffect(() => {
    // the effect is called twice on startup, so this check
    // was added in order to avoid stacking listeners for the
    // same event.
    if (resizeListenerSet) {
      return;
    }

    setWindowHeight(window.innerHeight);
    function handleResize() {
      setWindowHeight(window.innerHeight);
    }

    window.addEventListener('resize', handleResize)
    resizeListenerSet = true;
  }, [])

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