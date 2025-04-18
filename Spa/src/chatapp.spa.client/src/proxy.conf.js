const { env } = require('process');

const target = env["services__chatapp.spa-server__https__0"] ?? 'https://localhost:7224';

const PROXY_CONFIG = [
  {
    context: [
      "/api",
    ],
    target,
    ws: true,
    changeOrigin: true,
    secure: false
  }
]

module.exports = PROXY_CONFIG;
