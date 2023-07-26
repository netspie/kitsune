import React from "react";
import Dropdown from "./Dropdown";
import { DropdownItem } from "./Dropdown";

type DropdownLabeledProps = {
  label?: string;
  dropdownKey: string;
  selectedId: string;
  items: DropdownItem[];
  onItemClick?: (id: string) => void;
};

function DropdownLabeled(props: DropdownLabeledProps) {
  return (
    <div className="flex gap-4 h-fit items-center">
      {
        props.label && props.label.length !== 0 &&
        <label className="text-neutral-500 dark:text-neutral-400 justify-center h-fit">
          {props.label}
        </label>
      }
      <Dropdown
        dropdownKey={props.dropdownKey}
        selectedId={props.selectedId}
        items={props.items}
        onItemClick={props.onItemClick}
      />
    </div>
  );
}

export default DropdownLabeled;
