import { useState } from "react";
import { useTimeline } from "../../hooks/useTimeline";
import { useAuth } from "../../hooks/useAuth";
import { TimelineEntry } from "./TimelineEntry";
import { EntryFilter } from "./EntryFilter";
import { EntryModal } from "./EntryModal";
import { EntryForm } from "./EntryForm";
import type {
  TimelineEntry as TimelineEntryType,
  CreateTimelineEntry,
} from "../../types/TimelineEntry";

export const TimelineView = () => {
  const { user } = useAuth();
  const {
    entries,
    isLoading,
    error,
    fetchEntries,
    createEntry,
    updateEntry,
    deleteEntry,
    searchEntries,
  } = useTimeline();
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [editingEntry, setEditingEntry] = useState<TimelineEntryType | null>(
    null
  );

  const handleCreateEntry = async (entry: CreateTimelineEntry) => {
    await createEntry(entry);
    setIsModalOpen(false);
  };

  const handleUpdateEntry = async (entry: Partial<TimelineEntryType>) => {
    if (editingEntry) {
      await updateEntry(editingEntry.id, entry);
      setEditingEntry(null);
      setIsModalOpen(false);
    }
  };

  const handleDeleteEntry = async (id: number) => {
    if (window.confirm("Are you sure you want to delete this entry?")) {
      await deleteEntry(id);
    }
  };

  const handleEditClick = (entry: TimelineEntryType) => {
    setEditingEntry(entry);
    setIsModalOpen(true);
  };

  const handleModalClose = () => {
    setIsModalOpen(false);
    setEditingEntry(null);
  };

  const handleFilter = (query: string, category: string, entryType: string) => {
    searchEntries(query, category, entryType);
  };

  const handleResetFilter = () => {
    fetchEntries();
  };

  if (isLoading) {
    return (
      <div className="loading">
        <p>Loading timeline...</p>
      </div>
    );
  }

  if (error) {
    return (
      <div className="empty-state">
        <div className="empty-state-icon">⚠️</div>
        <h2 className="empty-state-title">Error</h2>
        <p className="empty-state-text">{error}</p>
        <button onClick={fetchEntries} className="btn btn-primary">
          Try Again
        </button>
      </div>
    );
  }

  return (
    <div>
      <EntryFilter onFilter={handleFilter} onReset={handleResetFilter} />

      {entries.length === 0 ? (
        <div className="empty-state">
          <div className="empty-state-icon">📝</div>
          <h2 className="empty-state-title">No Timeline Entries Yet</h2>
          <p className="empty-state-text">
            Start building your personal timeline by adding your first entry!
          </p>
          <button
            onClick={() => setIsModalOpen(true)}
            className="btn btn-primary btn-large"
          >
            ➕ Create First Entry
          </button>
        </div>
      ) : (
        <div>
          {entries.map((entry) => (
            <TimelineEntry
              key={entry.id}
              entry={entry}
              onEdit={handleEditClick}
              onDelete={handleDeleteEntry}
            />
          ))}
        </div>
      )}

      {entries.length > 0 && (
        <button
          onClick={() => setIsModalOpen(true)}
          className="btn btn-success btn-large"
          style={{ marginTop: "2rem" }}
        >
          ➕ Add New Entry
        </button>
      )}

      <EntryModal
        isOpen={isModalOpen}
        onClose={handleModalClose}
        title={
          editingEntry ? "Edit Timeline Entry" : "Create New Timeline Entry"
        }
      >
        <EntryForm
          entry={editingEntry}
          onSubmit={async (entry) => {
            if (editingEntry) {
              await handleUpdateEntry(entry as Partial<TimelineEntryType>);
            } else {
              await handleCreateEntry(entry as CreateTimelineEntry);
            }
          }}
          onCancel={handleModalClose}
          userId={user?.id || 1}
        />
      </EntryModal>
    </div>
  );
};
