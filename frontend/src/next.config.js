/** @type {import('next').NextConfig} */
console.info("Process environment: " + process.env.NODE_ENV);

// TLS configuration
if (process.env.NODE_ENV === 'development') {
  console.info("Setting server to ignore TLS certificate errors.");
  process.env.NODE_TLS_REJECT_UNAUTHORIZED = "0";
}
else {
  console.info("TLS settings set to default.");
}

// Rewrite rules
const rewriteRules = [];
if(process.env.NODE_ENV === 'development') {
  console.info("Adding development environment rewrite rules.");

  rewriteRules.push({
        source: '/graphql',
        destination: 'http://localhost:5000/graphql/',
      });
}

// next config
const nextConfig = {
  reactStrictMode: true,
  swcMinify: true,
  output: 'standalone',
  async rewrites() {
    return rewriteRules;
  }
}

module.exports = nextConfig
