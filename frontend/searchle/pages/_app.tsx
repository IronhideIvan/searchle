import '../styles/globals.scss';
import type { AppProps } from 'next/app';
import { NextUIProvider } from '@nextui-org/react';
import { DarkTheme } from '../themes/darkTheme';

function MyApp({ Component, pageProps }: AppProps) {
  return (
    <NextUIProvider theme={DarkTheme}>
      <Component {...pageProps} />
    </NextUIProvider>
  )

}

export default MyApp
