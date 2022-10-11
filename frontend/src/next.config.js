/** @type {import('next').NextConfig} */
console.info("Process environment: " + process.env.NODE_ENV);

const isDev = process.env.NODE_ENV === 'development';

// TLS configuration
if (isDev) {
  console.info("Setting server to ignore TLS certificate errors.");
  process.env.NODE_TLS_REJECT_UNAUTHORIZED = "0";
}
else {
  console.info("TLS settings set to default.");
}

// Rewrite rules for dev
const rewriteRules = [];
if(isDev) {
  console.info("Adding development environment rewrite rules.");

  rewriteRules.push({
        source: '/graphql',
        destination: 'http://localhost:5000/graphql/',
      });
}

// Set the asset path when in production
let nextAssetPath = '';
let hasAssetPath = false;
if(!isDev) {
  hasAssetPath = true;
  
  // Ideally this would be passed in as an environment variable or some other external property. However,
  // because the configuration is defined at build time for static files, it's non-trivial to implement
  // a dynamic asset path when dealing with a docker image. As a result, I'm just hardcoding the base path
  // for non-development environments and will deal with this later... if I ever even need to.
  nextAssetPath = '/searchle';

  console.info("Non-development environment detected. Setting asset and base path for static files. Path is '" + nextAssetPath + "'");

  // Kluge for hosting on Docker with NGINX reverse-proxy. When the application is hosted under a subroute,
  // such as "/searchle", we should theoretically only need to set the assetPath for static assets to be routed
  // properly... however... that's not the case. Setting the assetPath by itself doesn't seem to do anything. So
  // as a workaround I am setting the basePath, but because no pages actually exist under the new basePath (application is
  // hosted under localhost:<port>/) we'll get a 404 when navigating to the homepage. Adding this rewrite rule fixes things
  // by effectively mocking a redirect.
  rewriteRules.push({
    source: nextAssetPath,
    destination: '/'
  });
}

// next config
const nextConfig = {
  reactStrictMode: true,
  swcMinify: true,
  output: 'standalone',
  // basePath: hasAssetPath ? nextAssetPath : undefined,
  // assetPath: hasAssetPath ? nextAssetPath.concat('/') : undefined,
  async rewrites() {
    return rewriteRules;
  }
}

module.exports = nextConfig
