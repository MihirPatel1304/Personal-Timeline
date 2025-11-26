interface HeaderProps {
  title: string;
  subtitle?: string;
  action?: React.ReactNode;
}

export const Header = ({ title, subtitle, action }: HeaderProps) => {
  return (
    <div className="header">
      <div>
        <h1 className="header-title">{title}</h1>
        {subtitle && <p className="header-subtitle">{subtitle}</p>}
      </div>
      {action && <div>{action}</div>}
    </div>
  );
};
