import React from 'react';
import Document, { Html, Head, Main, NextScript, DocumentContext } from 'next/document';
import { CssBaseline, styled } from '@nextui-org/react';

const MyBody = styled('body',
  {
    backgroundColor: "$background",
    background: "$background",
    color: "$foreground"
  }
);

const MyHtml = styled(Html, {
  colorScheme: "$colorScheme"
});

class MyDocument extends Document {
  static async getInitialProps(ctx: DocumentContext) {
    const initialProps = await Document.getInitialProps(ctx);
    return {
      ...initialProps,
      styles: React.Children.toArray([initialProps.styles])
    };
  }

  render() {
    return (
      <MyHtml lang="en">
        <Head>{CssBaseline.flush()}</Head>
        <MyBody>
          <Main />
          <NextScript />
        </MyBody>
      </MyHtml>
    );
  }
}

export default MyDocument;