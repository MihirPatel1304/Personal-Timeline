import { useState, useEffect } from "react";
import type {
  TimelineEntry,
  CreateTimelineEntry,
} from "../../types/TimelineEntry";

interface EntryFormProps {
  entry?: TimelineEntry | null;
  onSubmit: (
    entry: CreateTimelineEntry | Partial<TimelineEntry>
  ) => Promise<void>;
  onCancel: () => void;
  userId: number;
}

export const EntryForm = ({
  entry,
  onSubmit,
  onCancel,
  userId,
}: EntryFormProps) => {
  const [formData, setFormData] = useState({
    title: "",
    description: "",
    eventDate: new Date().toISOString().split("T")[0],
    entryType: "Activity" as
      | "Achievement"
      | "Activity"
      | "Milestone"
      | "Memory",
    category: "",
    imageUrl: "",
    externalUrl: "",
  });
  const [isSubmitting, setIsSubmitting] = useState(false);

  useEffect(() => {
    if (entry) {
      setFormData({
        title: entry.title,
        description: entry.description,
        eventDate: new Date(entry.eventDate).toISOString().split("T")[0],
        entryType: entry.entryType,
        category: entry.category,
        imageUrl: entry.imageUrl || "",
        externalUrl: entry.externalUrl || "",
      });
    }
  }, [entry]);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setIsSubmitting(true);

    try {
      if (entry) {
        // Update existing entry
        await onSubmit({
          ...formData,
          eventDate: new Date(formData.eventDate).toISOString(),
        });
      } else {
        // Create new entry
        await onSubmit({
          userId,
          ...formData,
          eventDate: new Date(formData.eventDate).toISOString(),
        });
      }
    } catch (error) {
      console.error("Failed to submit entry:", error);
    } finally {
      setIsSubmitting(false);
    }
  };

  return (
    <form onSubmit={handleSubmit}>
      <div className="form-group">
        <label htmlFor="title" className="form-label">
          Title *
        </label>
        <input
          id="title"
          type="text"
          className="form-input"
          value={formData.title}
          onChange={(e) => setFormData({ ...formData, title: e.target.value })}
          required
        />
      </div>

      <div className="form-group">
        <label htmlFor="description" className="form-label">
          Description
        </label>
        <textarea
          id="description"
          className="form-textarea"
          value={formData.description}
          onChange={(e) =>
            setFormData({ ...formData, description: e.target.value })
          }
          rows={4}
        />
      </div>

      <div className="form-group">
        <label htmlFor="eventDate" className="form-label">
          Event Date *
        </label>
        <input
          id="eventDate"
          type="date"
          className="form-input"
          value={formData.eventDate}
          onChange={(e) =>
            setFormData({ ...formData, eventDate: e.target.value })
          }
          required
        />
      </div>

      <div className="form-group">
        <label htmlFor="entryType" className="form-label">
          Entry Type *
        </label>
        <select
          id="entryType"
          className="form-select"
          value={formData.entryType}
          onChange={(e) =>
            setFormData({
              ...formData,
              entryType: e.target.value as
                | "Achievement"
                | "Activity"
                | "Milestone"
                | "Memory",
            })
          }
          required
        >
          <option value="Activity">Activity</option>
          <option value="Achievement">Achievement</option>
          <option value="Milestone">Milestone</option>
          <option value="Memory">Memory</option>
        </select>
      </div>

      <div className="form-group">
        <label htmlFor="category" className="form-label">
          Category
        </label>
        <input
          id="category"
          type="text"
          className="form-input"
          value={formData.category}
          onChange={(e) =>
            setFormData({ ...formData, category: e.target.value })
          }
          placeholder="e.g., Work, Personal, Health"
        />
      </div>

      <div className="form-group">
        <label htmlFor="imageUrl" className="form-label">
          Image URL
        </label>
        <input
          id="imageUrl"
          type="url"
          className="form-input"
          value={formData.imageUrl}
          onChange={(e) =>
            setFormData({ ...formData, imageUrl: e.target.value })
          }
          placeholder="https://example.com/image.jpg"
        />
      </div>

      <div className="form-group">
        <label htmlFor="externalUrl" className="form-label">
          External Link
        </label>
        <input
          id="externalUrl"
          type="url"
          className="form-input"
          value={formData.externalUrl}
          onChange={(e) =>
            setFormData({ ...formData, externalUrl: e.target.value })
          }
          placeholder="https://example.com"
        />
      </div>

      <div className="timeline-actions">
        <button
          type="submit"
          className="btn btn-primary"
          disabled={isSubmitting}
        >
          {isSubmitting ? "Saving..." : entry ? "Update Entry" : "Create Entry"}
        </button>
        <button type="button" className="btn btn-secondary" onClick={onCancel}>
          Cancel
        </button>
      </div>
    </form>
  );
};
