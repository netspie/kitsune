'use client';
import { Card, Dropdown } from "../components";
import React from "react";

function MediaX() {
  return (
    <div className="mt-32">
      <div className="flex gap-4 h-12 lg:-mr-14 md:px-0">
        <Dropdown
          dropdownKey="1"
          selectedId="Ascending"
          items={["Ascending", "Descending"]}
          onItemClick={selected => {console.log(selected)}}
        />
        <Dropdown
          dropdownKey="2"
          selectedId="Anime"
          onItemClick={selected => {console.log(selected)}}
          items={[
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
