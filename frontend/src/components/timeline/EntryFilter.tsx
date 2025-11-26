import { useState } from "react";

interface EntryFilterProps {
  onFilter: (query: string, category: string, entryType: string) => void;
  onReset: () => void;
}

export const EntryFilter = ({ onFilter, onReset }: EntryFilterProps) => {
  const [query, setQuery] = useState("");
  const [category, setCategory] = useState("");
  const [entryType, setEntryType] = useState("");

  const handleFilter = () => {
    onFilter(query, category, entryType);
  };

  const handleReset = () => {
    setQuery("");
    setCategory("");
    setEntryType("");
    onReset();
  };

  return (
    <div className="filter-bar">
      <div className="filter-row">
        <div className="form-group" style={{ marginBottom: 0 }}>
          <input
            type="text"
            className="form-input"
            placeholder="Search titles and descriptions..."
            value={query}
            onChange={(e) => setQuery(e.target.value)}
            onKeyPress={(e) => e.key === "Enter" && handleFilter()}
          />
        </div>

        <div className="form-group" style={{ marginBottom: 0 }}>
          <input
            type="text"
            className="form-input"
            placeholder="Category (e.g., Work)"
            value={category}
            onChange={(e) => setCategory(e.target.value)}
            onKeyPress={(e) => e.key === "Enter" && handleFilter()}
          />
        </div>

        <div className="form-group" style={{ marginBottom: 0 }}>
          <select
            className="form-select"
            value={entryType}
            onChange={(e) => setEntryType(e.target.value)}
          >
            <option value="">All Types</option>
            <option value="Activity">Activity</option>
            <option value="Achievement">Achievement</option>
            <option value="Milestone">Milestone</option>
            <option value="Memory">Memory</option>
          </select>
        </div>

        <div style={{ display: "flex", gap: "0.5rem" }}>
          <button onClick={handleFilter} className="btn btn-primary">
            🔍 Search
          </button>
          <button onClick={handleReset} className="btn btn-secondary">
            Reset
          </button>
        </div>
      </div>
    </div>
  );
};
