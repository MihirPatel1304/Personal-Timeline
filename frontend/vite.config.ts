import { defineConfig } from "vite";
import react from "@vitejs/plugin-react";
import fs from "fs";

export default defineConfig({
  plugins: [
    react(),
    {
      name: "hide-localhost",
      configureServer(server) {
        const originalPrintUrls = server.printUrls;
        server.printUrls = function () {
          const info = server.config.logger.info;
          server.config.logger.info = (msg) => {
            if (!msg.includes("localhost")) {
              info(msg);
            }
          };
          originalPrintUrls.call(server);
          server.config.logger.info = info;
        };
      },
    },
  ],
  server: {
    https: {
      key: fs.readFileSync("./ssl/localhost+2-key.pem"),
      cert: fs.readFileSync("./ssl/localhost+2.pem"),
    },
    host: "127.0.0.1",
    port: 5173,
    strictPort: true,
  },
});
