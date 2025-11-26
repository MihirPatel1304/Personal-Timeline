import type { TimelineEntry as TimelineEntryType } from "../../types/TimelineEntry";
import { format } from "date-fns";

interface TimelineEntryProps {
  entry: TimelineEntryType;
  onEdit: (entry: TimelineEntryType) => void;
  onDelete: (id: number) => void;
}

export const TimelineEntry = ({
  entry,
  onEdit,
  onDelete,
}: TimelineEntryProps) => {
  const getEntryTypeBadgeClass = (type: string) => {
    const baseClass = "timeline-badge";
    switch (type) {
      case "Achievement":
        return `${baseClass} timeline-badge-achievement`;
      case "Milestone":
        return `${baseClass} timeline-badge-milestone`;
      case "Activity":
        return `${baseClass} timeline-badge-activity`;
      case "Memory":
        return `${baseClass} timeline-badge-memory`;
      default:
        return baseClass;
    }
  };

  const getSourceIcon = (source: string) => {
    switch (source) {
      case "GitHub":
        return "⚙️";
      case "Spotify":
        return "🎵";
      case "Discord":
        return "💬";
      default:
        return "✍️";
    }
  };

  return (
    <div className="timeline-card">
      <div className="timeline-card-header">
        <div className="timeline-card-header-left">
          <span className={getEntryTypeBadgeClass(entry.entryType)}>
            {entry.entryType}
          </span>
          <span className="timeline-category">{entry.category}</span>
          <span className="timeline-source">
            {getSourceIcon(entry.sourceApi)} {entry.sourceApi}
          </span>
        </div>
        <div className="timeline-date">
          {format(new Date(entry.eventDate), "MMM dd, yyyy")}
        </div>
      </div>

      <h3 className="timeline-title">{entry.title}</h3>
      <p className="timeline-description">{entry.description}</p>

      {entry.imageUrl && (
        <img
          src={entry.imageUrl}
          alt={entry.title}
          className="timeline-image"
        />
      )}

      {entry.externalUrl && (
        <a
          href={entry.externalUrl}
          target="_blank"
          rel="noopener noreferrer"
          className="timeline-link"
        >
          🔗 View External Link
        </a>
      )}

      <div className="timeline-actions">
        <button onClick={() => onEdit(entry)} className="btn btn-primary">
          Edit
        </button>
        <button onClick={() => onDelete(entry.id)} className="btn btn-danger">
          Delete
        </button>
      </div>
    </div>
  );
};
