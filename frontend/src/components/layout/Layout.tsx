import type { ReactNode } from "react";
import { Navigation } from "./Navigation";

interface LayoutProps {
  children: ReactNode;
}

export const Layout = ({ children }: LayoutProps) => {
  return (
    <div className="layout-container">
      <Navigation />
      <main className="layout-main">
        <div className="layout-content">{children}</div>
      </main>
      <footer className="layout-footer">
        <p>© Personal Timeline Project. Built with React & .NET</p>
      </footer>
    </div>
  );
};
