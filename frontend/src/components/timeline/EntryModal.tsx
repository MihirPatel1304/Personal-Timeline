import type { ReactNode } from "react";

interface EntryModalProps {
  isOpen: boolean;
  onClose: () => void;
  title: string;
  children: ReactNode;
}

export const EntryModal = ({
  isOpen,
  onClose,
  title,
  children,
}: EntryModalProps) => {
  if (!isOpen) return null;

  return (
    <div className="modal-overlay" onClick={onClose}>
      <div className="modal-content" onClick={(e) => e.stopPropagation()}>
        <div className="modal-header">
          <h2 className="modal-title">{title}</h2>
          <button className="modal-close" onClick={onClose}>
            ×
          </button>
        </div>
        {children}
      </div>
    </div>
  );
};
