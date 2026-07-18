const { env } = require('process');

const target = env.services__api__https__0
  || env.services__api__http__0
  || 'https://localhost:57679';
console.log('--- Proxy Target Configured To: ---', target);

const PROXY_CONFIG = [
  {
    context: ['/api'],
    target,
    secure: false,
    changeOrigin: true,
    pathRewrite: { '^/api': '' }
  }
];

module.exports = PROXY_CONFIG;
