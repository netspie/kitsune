'use client';
import { Card, DropdownLabeled } from "../components";
import React from "react";

function MediaX() {
  return (
    <div className="mt-32 xl:mx-64">
      <div className="flex gap-6 h-12 lg:-mr-14 md:px-0">
        <DropdownLabeled
          label="Sort By"
          dropdownKey="1"
          selectedId="Author"
          items={["Creation", "Author", "Item Name"]}
          onItemClick={selected => {console.log(selected)}}
        />
        <DropdownLabeled
          label=""
          dropdownKey="2"
          selectedId="Ascending"
          items={["Ascending", "Descending"]}
          onItemClick={selected => {console.log(selected)}}
        />
        <DropdownLabeled
          label="Filter By"
          dropdownKey="3"
          selectedId="Anime"
          onItemClick={selected => {console.log(selected)}}
          items={[
            "None",
            "Anime",
            "Drama",
            "TV Show",
            "Manga",
            "Newspaper",
            "Conversation",
          ]}
        />
      </div>
      <div className="flex flex-col gap-6">
        <Card
          name="Dragon Ball - Episode 43"
          type="Anime"
          creation="Dragon Ball"
          author="Akira Toriyama"
        />
        <Card
          name="Naruto - Episode 2"
          type="Anime"
          creation="Naruto"
          author="Someone"
        />
        <Card
          name="Yaki Yakitori - Episode 3"
          type="Drama"
          creation="Yakie Yakitori"
          author="A Director"
        />
        <Card
          name="Yuta Aoki - Vocab Conversations - 34 / 32"
          type="Conversation"
          creation="Yuta Aoki - Vocab Conversations"
          author="Yuta Aoki "
        />
      </div>
    </div>
  );
}

export default MediaX;
