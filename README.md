# Kitsune Study

&nbsp;&nbsp;&nbsp;&nbsp; Japanese language learning app where you can add and explore various materials and turn anything into flashcards.

## General

#### Technologies
&nbsp;&nbsp;&nbsp;&nbsp; NET Core, C#, HTML, CSS, Blazor Server, MongoDb, AWS, Docker

#### Demo Urls
&nbsp;&nbsp;&nbsp;&nbsp; https://kitsunestudy.net

#### High Level Features

- Create, edit and explore learning content - courses, lessons, conversations, phrases, vocabulary and various media
- Turn anything into a flashcard list in desired, customizable form - for reading, listening, speaking, writing practice
- Track progress of learned material, adjust for own needs

#### Goals

For a japanese language learners - address the following problems (just some among many):  
  
- Writing notes from videos, courses, articles, lectures - which takes effort and time.  
- Material fatigue - lack of innovation of learning process and revisions, which leads to procrastination.  
- Lack of tools, ways for learning material from different angles, ex. reading, listening, speaking, writing.  
- Learning materials scattered all over the internet, instead of in the single place.  
- Hard to get back and rehearse your knowledge in an easy way. No automation. No challenging tasks.  
- If there are high quality materials, then they are costly.  

## Design

#### User Scenarios

##### Case 1 - Learn the language from scratch in an organized way

*Steps*
- Go to courses page and choose course of appropriate difficulty level
- Read, listen, learn from a lesson, mark it for further rehearse
- Rehearse learned material based on spaced repetition algorithm or customized

##### Case 2 - Learn specific piece of language (grammar point, writing script characters, words)

*Steps*
- Find a specific grammar point, word, or phrase and learn it
- Mark it to remember for rehearse
- Rehearse learned material based on spaced repetition algorithm or customized

##### Case 3 - Learn specific piece of media (movie subtitles, song lyrics, situational phrases?)

*Steps*
- similar as before

##### Case 4 - Learn vocabulary or phrases related to specific situation or thematics

*Steps*
- similar as before

##### Case 5 - Rehearse learned material

*Steps*
- Go to rehearse page
- Define rehearse material range from filter options:
  - spaced-repetition - auto - no filters selected
  - root item - a specific item or item type containing target items you want to rehearse - could be a lesson, conversation, specific song lyrics and so on
  - target item type - phrase, word, character
- Choose rehearse mode - reading, listening, speaking, writing
- Choose flashcard list count limit for single session
- Rehearse

## Materials Structure

- Official Courses
  - Lessons - **Done**
    - Conversations
    - Phrases
    - Situations
    - Articles
    - Vocabulary
    - Grammar points
- Writing Scripts
  - Kana
    - Hiragana 
    - Katakana
  - Kanji
    - Kanji Characters
    - Radicals
  - Words
- JLPT Categorization
  - N5, N4, N3..
- Vocabulary
  - By Linguistics
    - Verbs, Nouns, Adjectives, Particles
      - Word Dictionary Writings
        - Word Meanings
        - Word Inflections
        - Word Compounds
        - 
  - By Theme
    - Colors, Numbers, Animals, Names and tons more..
  - Similar words
    - By similar but not exactly the same writing, sound, ex. 怖い - 可愛い
    - By the same kana writing, sound, but different pitches, ex. 洗車 - 戦車 or 橋 - 箸
    - By same writing, sounds, almost the same meanings but just little bit niuanced, ex. 思い -　想い
- Writing type preference
  - Usually kana (hiragana) alone
  - Usually kanji (and kana of course, depending on a word)
  - Katakana (not just foreign words, but also normal words but to make them look different, or titles)
  - Character
    - Hiragana Character
    - Katakana Character
    - Kanji Character
    - Radical Character
- Phrases
  - Popular phrases
  - Situations
  - Level of difficulty (simple/beginner, advanced..)
  - Levels of politeness (rude, informal, formal, humble/honorific)
- Media
  - Anime, manga, songs, articles, arts, stories, TV dramas
- Genders - vocabulary or phrases used depending on a person's gender
- Age jargons - vocabulary or phrases used depending on a person's age
- Keigo - rude, informal, formal, humble/honorific
- Dialects - Kanto, Kansai, Kyushuu and more..
