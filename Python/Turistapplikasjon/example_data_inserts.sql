-- -----------------------------------------------------
-- Example data for table `turistdb`.`rewards`
-- -----------------------------------------------------
INSERT INTO `turistdb`.`rewards` (reward_type, details) 
VALUES ( 'Gift Card', 'You have earned a $10 gift card for completing 5 achievements!');


-- -----------------------------------------------------
-- Example data for table `turistdb`.`blocked_username`
-- -----------------------------------------------------
INSERT INTO `turistdb`.`blocked_username` (`id`, `blocked_username`) 
VALUES 
    (1, 'blocked'),
    (2, 'blocked1');


-- -----------------------------------------------------
-- Example data for table `turistdb`.`achievement`
-- -----------------------------------------------------
INSERT INTO `turistdb`.`achievement` (`id`, `definition`, `points_required`, `data`, `icon`) 
VALUES
    (1, 'First Achievement', 50, 'Congratulations! You have earned your first achievement.', 'location.png'),
    (2, '2nd Achievement', 100, 'test.', 'target.png'),
    (3, '3rd Achievement', 200, 'test.', 'diamond.png'),
    (4, '4th Achievement', 300, 'test.', 'star.png'),
    (5, '5th Achievement', 500, 'test.', 'achievement.png');


-- -----------------------------------------------------
-- Example data for table `turistdb`.`city`
-- -----------------------------------------------------
INSERT INTO `turistdb`.`city` (`id`, `name`) VALUES (1, 'Oslo');
INSERT INTO `turistdb`.`city` (`id`, `name`) VALUES (2, 'Bergen');


-- -----------------------------------------------------
-- Example data for table `turistdb`.`attraction` Generated with CHATGPT:
-- -----------------------------------------------------
INSERT INTO `turistdb`.`attraction` (`id`, `city_id`, `name`, `practical_information`, `short_description`) 
VALUES
(1, 1, 'Oslo Opera House', '{"description": "The Oslo Opera House is an architectural marvel located by the waterfront in the Bjørvika district of Oslo. With its unique and modern design, it exudes a sense of being both majestic and approachable at the same time. The Opera House is not just a cultural icon, but also a symbol of Oslo''s creative and contemporary identity. Visitors can experience everything from spectacular opera productions and ballet performances to concerts, theater shows, and art exhibitions. The large glass facade also offers stunning views of the Oslo Fjord and the surrounding cityscape.", "opening_hours": "Monday–Saturday 11:00 - 22:00, Sunday 12:00–22:00", "entry_fee": "Variable ticket prices. Standing area 100 NOK,- for most performances. See official website for more information.", "nearest_public_transport_directions": "https://maps.app.goo.gl/37FvF2Z5HFs43HEd9", "google_maps_location": "https://maps.app.goo.gl/3ErX7uyDrGsQveQM9", "official_website": "https://operaen.no/"}', 'Its accessible roof and broad open public lobbies make the building a social monument rather than a sculptural one.'),
(2, 1, 'Akershus Fortress', '{"description": "Akershus Fortress is a historic attraction situated in the heart of Oslo, Norway. With roots dating back to the 13th century, it stands as a testament to Norway''s rich cultural heritage and storied past. The fortress overlooks the Oslo Fjord, offering panoramic views of the surrounding landscape. As visitors approach Akershus Fortress, they are greeted by imposing stone walls and medieval architecture, creating an atmosphere of intrigue and grandeur. Within its walls lie centuries of history, from its origins as a royal residence to its role as a military stronghold and a symbol of Norwegian sovereignty. Today, Akershus Fortress serves as a popular tourist attraction and cultural center. Visitors can explore its labyrinthine passages, fortified towers, and historic buildings, gaining insight into Norway''s medieval past. The fortress also houses museums and exhibitions that delve into various aspects of Norwegian history, including military history, royal life, and archaeological discoveries. Beyond its historical significance, Akershus Fortress offers a picturesque setting for leisurely strolls, picnics, and scenic photography. Its location near the city center makes it easily accessible for locals and tourists alike, providing a serene escape from the bustle of urban life.", "opening_hours": "All days: 06:00 - 21:00", "entry_fee": "Free", "nearest_public_transport_directions": "https://maps.app.goo.gl/VVZAq2e4W7BLfSDZ9", "google_maps_location": "https://maps.app.goo.gl/X2ZxJa9kn6D5wyax8", "official_website": "https://kultur.forsvaret.no/forsvarets-festninger/akershus-festning"}', 'A vital stronghold and royal residence since the 14th century.'),
(3, 1, 'Munch Museum', '{"description": "Located on Oslo''s waterfront, MUNCH is the new home to the world''s largest collection of works by Norwegian artist Edvard Munch, featuring iconic pieces like ''The Scream''. The museum provides not only a deep dive into Munch''s life and art but also hosts various exhibitions from international artists. The architecture of the museum itself is a modern marvel, designed to provide an immersive experience.", "opening_hours": "Monday–Friday 10:00 - 16:00, Saturday–Sunday 10:00 - 17:00", "entry_fee": "Adults: 120 NOK, Students: 60 NOK, Children under 16: Free", "nearest_public_transport_directions": "https://maps.app.goo.gl/Bv2Q69hMzRaLxnDM7", "google_maps_location": "https://maps.app.goo.gl/snL1FftSXQdgkFg37", "official_website": "https://www.munchmuseet.no"}', 'Home to the world''s largest collection of Edvard Munch''s works.'),
(4, 1, 'Vigeland Park', '{"description": "Vigeland Park, or Frogner Park, is the largest park in Oslo and known for its collection of sculptures by Gustav Vigeland. This includes more than 200 sculptures in bronze, granite, and wrought iron. The park is open to the public at all times and is a major cultural monument to Norwegian sculptural heritage.", "opening_hours": "Open 24 hours", "entry_fee": "Free", "nearest_public_transport_directions": "https://maps.app.goo.gl/puGSLDR5Pwh5gU7R9", "google_maps_location": "https://maps.app.goo.gl/KqBqsTq4Wx8HSQZC7", "official_website": "https://vigeland.museum.no/"}', 'The world''s largest sculpture park made by a single artist.'),
(5, 2, 'Ulriken', '{"description": "Ulriken, towering over Bergen, is the highest of the citys seven mountains, offering breathtaking panoramic views of the city, fjords, and surrounding islands. Adventurers can explore hiking trails leading to the summit, while others can enjoy a scenic cable car ride. At the top, visitors can dine at the mountain restaurant, embark on paragliding adventures, or simply take in the majestic scenery.", "opening_hours": "Varies by season, typically 09:00 AM to 10:00 PM in summer and 10:00 AM to 04:00 PM in winter", "entry_fee": "Varies by activity, approximately 225 NOK to 250 NOK for cable car round-trip ticket", "nearest_public_transport_directions": "https://goo.gl/maps/zKUM94tKZVMgBcVu9", "google_maps_location": "https://goo.gl/maps/gsngP5y4b7pJZaCw8", "official_website": "https://www.ulriken643.no/en"}', 'Bergens tallest peak, offering unparalleled views and outdoor adventures.');

-- -----------------------------------------------------
-- Example data for table `turistdb`.`user`
-- -----------------------------------------------------
INSERT INTO `turistdb`.`user` (`id`, `username`, `first_name`, `last_name`, `email`, `password_hash`, `verified`, `is_admin`, `is_blocked`, `points`) 
VALUES 
    (2, 'test_user_1', 'test', 'test', 'test_1@t.no', 'scrypt:32768:8:1$RfAzU7ufbIstpY5r$fe56b6d1bb252e5faef5960e190830e5cf552f6649e1db0e1c0d638cd77cf5d31a4a9859d435a5a4b7c476cdba7239f5200a7c7fe7aa04d562a9b12272afbcfc', 0, 0, 0, 5000),
    (3, 'test_user_2', 'test', 'test', 'test_2@t.no', 'scrypt:32768:8:1$RfAzU7ufbIstpY5r$fe56b6d1bb252e5faef5960e190830e5cf552f6649e1db0e1c0d638cd77cf5d31a4a9859d435a5a4b7c476cdba7239f5200a7c7fe7aa04d562a9b12272afbcfc', 0, 0, 0, 2500),
    (4, 'test_user_3', 'test', 'test', 'test_3@t.no', 'scrypt:32768:8:1$RfAzU7ufbIstpY5r$fe56b6d1bb252e5faef5960e190830e5cf552f6649e1db0e1c0d638cd77cf5d31a4a9859d435a5a4b7c476cdba7239f5200a7c7fe7aa04d562a9b12272afbcfc', 0, 0, 0, 1000),
    (5, 'test_user_4', 'test', 'test', 'test_4@t.no', 'scrypt:32768:8:1$RfAzU7ufbIstpY5r$fe56b6d1bb252e5faef5960e190830e5cf552f6649e1db0e1c0d638cd77cf5d31a4a9859d435a5a4b7c476cdba7239f5200a7c7fe7aa04d562a9b12272afbcfc', 0, 0, 0, 500),
    (6, 'test_user_5', 'test', 'test', 'test_5@t.no', 'scrypt:32768:8:1$RfAzU7ufbIstpY5r$fe56b6d1bb252e5faef5960e190830e5cf552f6649e1db0e1c0d638cd77cf5d31a4a9859d435a5a4b7c476cdba7239f5200a7c7fe7aa04d562a9b12272afbcfc', 0, 0, 0, 350),
    (7, 'test_user_6', 'test', 'test', 'test_6@t.no', 'scrypt:32768:8:1$RfAzU7ufbIstpY5r$fe56b6d1bb252e5faef5960e190830e5cf552f6649e1db0e1c0d638cd77cf5d31a4a9859d435a5a4b7c476cdba7239f5200a7c7fe7aa04d562a9b12272afbcfc', 0, 0, 0, 1200);

-- -----------------------------------------------------
-- Example data for table `turistdb`.`user_has_level`
-- -----------------------------------------------------
INSERT INTO `turistdb`.`user_has_level` (`user_id`, `attraction_id`, `level`) 
VALUES 
    (1, 1, 1), (1, 2, 1), (1, 3, 1), (1, 4, 1), (1, 5, 1),
    (2, 1, 1), (2, 2, 1), (2, 3, 1), (2, 4, 1), (2, 5, 1),
    (3, 1, 1), (3, 2, 1), (3, 3, 1), (3, 4, 1), (3, 5, 1),
    (4, 1, 1), (4, 2, 1), (4, 3, 1), (4, 4, 1), (4, 5, 1),
    (5, 1, 1), (5, 2, 1), (5, 3, 1), (5, 4, 1), (5, 5, 1),
    (6, 1, 1), (6, 2, 1), (6, 3, 1), (6, 4, 1), (6, 5, 1),
    (7, 1, 1), (7, 2, 1), (7, 3, 1), (7, 4, 1), (7, 5, 1);


-- -----------------------------------------------------
-- Example data for table `turistdb`.`attraction_has_category`
-- -----------------------------------------------------
INSERT INTO `turistdb`.`attraction_has_category` (`attraction_id`, `category_id`) VALUES (3, 1);
INSERT INTO `turistdb`.`attraction_has_category` (`attraction_id`, `category_id`) VALUES (1, 3);
INSERT INTO `turistdb`.`attraction_has_category` (`attraction_id`, `category_id`) VALUES (4, 4);
INSERT INTO `turistdb`.`attraction_has_category` (`attraction_id`, `category_id`) VALUES (2, 5);
INSERT INTO `turistdb`.`attraction_has_category` (`attraction_id`, `category_id`) VALUES (3, 5);
INSERT INTO `turistdb`.`attraction_has_category` (`attraction_id`, `category_id`) VALUES (4, 8);
INSERT INTO `turistdb`.`attraction_has_category` (`attraction_id`, `category_id`) VALUES (2, 9);
INSERT INTO `turistdb`.`attraction_has_category` (`attraction_id`, `category_id`) VALUES (1, 10);


-- -----------------------------------------------------
-- Example data : level 1 questions for Opera Opera House. Generated with CHATGPT:
-- -----------------------------------------------------
INSERT INTO `turistdb`.`question` (`id`, `attraction_id`,`level`, `question_text`,`correct_answer`, `option_2`,`option_3`, `option_4`, `option_5`)
VALUES
    (1, 1, 1, 'What year was the Oslo Opera House officially opened?', '2008', '2004', '2006', '2010', '2012'),
    (2, 1, 1, 'Who was the architect behind the design of the Oslo Opera House?', 'Snøhetta', 'Frank Gehry', 'Zaha Hadid', 'Renzo Piano', 'Norman Foster'),
    (3, 1, 1, 'Which body of water does the Oslo Opera House overlook?', 'Oslo Fjord', 'North Sea', 'Baltic Sea', 'Norwegian Sea', 'Skagerrak'),
    (4, 1, 1, 'What material is predominantly used in the construction of the Oslo Opera House''s exterior?', 'Concrete', 'Glass', 'Steel', 'Wood', 'Marble'),
    (5, 1, 1, 'How many seats does the main auditorium of the Oslo Opera House have?', '2,100', '800', '1,100', '1,500', '2,500'),
    (6, 1, 1, 'Which of the following features is NOT part of the Oslo Opera House''s design?', 'Underground parking garage', 'Sloping roof', 'Vertical garden', 'Public terrace', 'Reflective facade'),
    (7, 1, 1, 'What cultural institutions are housed within the Oslo Opera House?', 'Opera, ballet, and symphony orchestra', 'Opera only', 'Ballet only', 'Symphony orchestra only', 'Opera and ballet'),
    (8, 1, 1, 'Which Norwegian king officially opened the Oslo Opera House?', 'King Harald V', 'King Olav V', 'King Haakon VII', 'King Magnus IV', 'King Olav II'),
    (9, 1, 1, 'What is the approximate total cost of constructing the Oslo Opera House?', '$800 million', '$200 million', '$400 million', '$600 million', '$1 billion'),
    (10, 1, 1, 'Which of the following events is NOT held at the Oslo Opera House?', 'Rock concerts', 'Opera performances', 'Ballet performances', 'Classical music concerts', 'Rock concerts'),
    (11, 1, 1, 'What is the nickname often used to refer to the Oslo Opera House?', 'The White Swan', 'The Black Pearl', 'The Crystal Palace', 'The Opera Pearl', 'The Glass House'),
    (12, 1, 1, 'What is the name of the area surrounding the Oslo Opera House?', 'Bjørvika', 'Opera District', 'Opera Park', 'Opera Square', 'Opera Bay'),
    (13, 1, 1, 'How many floors does the Oslo Opera House have?', '3', '1', '2', '4', '5'),
    (14, 1, 1, 'What type of performances are typically held at the Oslo Opera House?', 'Opera and ballet', 'Traditional Norwegian folk music', 'Experimental theater', 'Shakespearean plays', 'Opera and ballet'),
    (15, 1, 1, 'Which famous author wrote about the Oslo Opera House in his novel "The Snowman"?', 'Jo Nesbø', 'Stieg Larsson', 'Karl Ove Knausgård', 'Henning Mankell', 'Jørn Lier Horst');


-- -----------------------------------------------------
-- Example data : level 2 questions for Opera Opera House. Generated with CHATGPT:
-- -----------------------------------------------------
INSERT INTO `turistdb`.`question` (`id`, `attraction_id`,`level`, `question_text`,`correct_answer`, `option_2`,`option_3`, `option_4`, `option_5`)
VALUES
    (16, 1, 2, 'Which famous opera house inspired the design of the Oslo Opera House?', 'Sydney Opera House', 'La Scala', 'Royal Opera House (Covent Garden)', 'Vienna State Opera', 'Palais Garnier'),
    (17, 1, 2, 'What is the name of the large marble sculpture located outside the Oslo Opera House?', 'She Lies', 'Agora', 'The Thinker', 'The Kiss', 'The Discus Thrower'),
    (18, 1, 2, 'Who composed the inaugural opera performed at the Oslo Opera House?', 'Wolfgang Amadeus Mozart', 'Giuseppe Verdi', 'Richard Wagner', 'Giacomo Puccini', 'Claude Debussy'),
    (19, 1, 2, 'What is the name of the main performance hall inside the Oslo Opera House?', 'Main Stage', 'Grand Theater', 'Opera Hall', 'Concert Hall', 'Symphony Hall'),
    (20, 1, 2, 'Which architectural feature of the Oslo Opera House is said to represent an iceberg floating in the fjord?', 'The sloping roof', 'The glass facade', 'The cantilevered balconies', 'The underground foyer', 'The central atrium'),
    (21, 1, 2, 'In which district of Oslo is the Oslo Opera House located?', 'Bjørvika', 'Frogner', 'Grünerløkka', 'Majorstuen', 'Sagene'),
    (22, 1, 2, 'What was the original purpose of the land on which the Oslo Opera House now stands?', 'A shipyard', 'A royal palace', 'A fish market', 'A military barracks', 'A botanical garden'),
    (23, 1, 2, 'Which prestigious international architecture award did the Oslo Opera House win in 2009?', 'Mies van der Rohe Award', 'Pritzker Prize', 'Stirling Prize', 'Driehaus Prize', 'Aga Khan Award'),
    (24, 1, 2, 'What is the name of the Norwegian national opera company based at the Oslo Opera House?', 'Den Norske Opera & Ballett', 'Det Norske Teatret', 'Nationaltheatret', 'Opera Vest', 'Bergen National Opera'),
    (25, 1, 2, 'Which Norwegian monarch is depicted in the large mosaic located in the foyer of the Oslo Opera House?', 'King Olav V', 'King Haakon VII', 'King Harald V', 'King Olav II', 'King Magnus IV'),
    (26, 1, 2, 'What is the name of the pedestrian bridge that connects the Oslo Opera House to the Barcode Project?', 'Bispevika Bridge', 'Aker Brygge Bridge', 'Tjuvholmen Bridge', 'Lambertseter Bridge', 'Gamlebyen Bridge'),
    (27, 1, 2, 'Which world-renowned conductor served as the Oslo Philharmonic Orchestra''s artistic director from 2000 to 2009?', 'Jukka-Pekka Saraste', 'Mariss Jansons', 'Herbert Blomstedt', 'Vasily Petrenko', 'Alan Gilbert'),
    (28, 1, 2, 'What is the name of the glass sculpture located in the lobby of the Oslo Opera House?', 'Kalvøya', 'Svartlamon', 'Sørenga', 'Nesodden', 'Sørlandet'),
    (29, 1, 2, 'What is the name of the Norwegian ballet company based at the Oslo Opera House?', 'The Norwegian National Ballet', 'Ballett Norge', 'Oslo Ballet Company', 'Norwegian Dance Theatre', 'The Royal Norwegian Ballet'),
    (30, 1, 2, 'Which architectural element of the Oslo Opera House is designed to invite visitors to walk on it and enjoy views of the city and fjord?', 'The sloping roof', 'The glass facade', 'The cantilevered balconies', 'The underground foyer', 'The central atrium');


-- -----------------------------------------------------
-- Example data : level 1 questions for Akerhus Fortress. Generated with CHATGPT:
-- -----------------------------------------------------
INSERT INTO `turistdb`.`question` (`id`, `attraction_id`,`level`, `question_text`,`correct_answer`, `option_2`,`option_3`, `option_4`, `option_5`)
VALUES
    (31, 2, 1, 'When was the construction of Akershus Fortress completed?', '1299', '1399', '1499', '1599', '1699'),
    (32, 2, 1, 'Who initiated the construction of Akershus Fortress?', 'King Haakon V', 'King Olaf II', 'King Harald Hardrada', 'King Magnus VI', 'King Christian IV'),
    (33, 2, 1, 'What body of water does Akershus Fortress overlook?', 'Oslofjord', 'North Sea', 'Baltic Sea', 'Skagerrak', 'Norwegian Sea'),
    (34, 2, 1, 'What is the name of the main entrance gate to Akershus Fortress?', 'Kings Gate', 'Castle Gate', 'Fortress Gate', 'Citadel Gate', 'Vardøhus Gate'),
    (35, 2, 1, 'What architectural style primarily characterizes Akershus Fortress?', 'Medieval', 'Renaissance', 'Baroque', 'Rococo', 'Neoclassical'),
    (36, 2, 1, 'Which Norwegian king expanded and fortified Akershus Fortress into its present form?', 'King Christian IV', 'King Haakon VI', 'King Olaf III', 'King Magnus VII', 'King Frederick II'),
    (37, 2, 1, 'What does "Akershus" mean in Norwegian?', '"Old Castle"', '"Water Castle"', '"Harbor Castle"', '"Royal Fortress"', '"Oak Castle"'),
    (38, 3, 1, 'What function did Akershus Fortress primarily serve during the Middle Ages?', 'Royal Residence', 'Prison', 'Government Center', 'Military Headquarters', 'Trading Post'),
    (39, 2, 1, 'What is the name of the medieval castle within Akershus Fortress?', 'Royal Keep', "Knight's Tower", "King's Hall", "Queen's Palace", "Bishop's Residence"),
    (40, 2, 1, 'In which modern-day borough of Oslo is Akershus Fortress located?', 'Gamle Oslo', 'Sentrum', 'Grünerløkka', 'Frogner', 'Nordstrand'),
    (41, 2, 1, 'Which European city served as the architectural inspiration for Akershus Fortress?', 'Stockholm', 'Copenhagen', 'Edinburgh', 'Prague', 'Berlin'),
    (42, 2, 1, 'During which historical event was Akershus Fortress expanded and fortified into its present form?', 'Union between Norway and Denmark', 'Battle of Hafrsfjord', 'Black Death pandemic', 'Founding of Oslo as the capital', 'Reign of King Haakon V'),
    (43, 2, 1, 'Which monarch ordered the construction of Akershus Fortress?', 'King Haakon V', 'King Olaf II', 'King Harald Hardrada', 'King Magnus VI', 'King Christian IV'),
    (44, 2, 1, 'Which famous Norwegian playwright and poet was imprisoned at Akershus Fortress in the 19th century for his political activities?', 'Henrik Wergeland', 'Henrik Ibsen', 'Bjørnstjerne Bjørnson', 'Knut Hamsun', 'Alexander Kielland'),
    (45, 2, 1, 'Which body of water does Akershus Fortress overlook?', 'Oslofjord', 'North Sea', 'Baltic Sea', 'Skagerrak', 'Norwegian Sea');


-- -----------------------------------------------------
-- Example data : level 1 questions for Munch Museum. Generated with CHATGPT:
-- -----------------------------------------------------
INSERT INTO `turistdb`.`question` (`id`, `attraction_id`,`level`, `question_text`,`correct_answer`, `option_2`,`option_3`, `option_4`, `option_5`)
VALUES
    (46, 3, 1, 'When was the Oslo Munch Museum founded?', '1963', '1973', '1983', '1993', '2003'),
    (47, 3, 1, 'What is the primary focus of the Oslo Munch Museum?', 'Works by Edvard Munch', 'Renaissance art', 'Modern sculpture', 'Ancient artifacts', 'Photography'),
    (48, 3, 1, 'In which European city is the Oslo Munch Museum located?', 'Oslo', 'Copenhagen', 'Stockholm', 'Berlin', 'Paris'),
    (49, 3, 1, 'Which famous Norwegian artist is the museum named after?', 'Edvard Munch', 'Henrik Ibsen', 'August Strindberg', 'Gustav Vigeland', 'Fridtjof Nansen'),
    (50, 3, 1, 'What is the most famous painting by Edvard Munch housed in the Oslo Munch Museum?', 'The Scream', 'Mona Lisa', 'Starry Night', 'The Last Supper', 'The Persistence of Memory'),
    (51, 3, 1, 'Which of the following is NOT a medium in which Edvard Munch worked?', 'Sculpture', 'Painting', 'Printmaking', 'Drawing', 'Photography'),
    (52, 3, 1, 'In which neighborhood of Oslo is the Oslo Munch Museum located?', 'Tøyen', 'Grünerløkka', 'Frogner', 'Majorstuen', 'Sentrum'),
    (53, 3, 1, 'How many versions of "The Scream" are housed in the Oslo Munch Museum?', 'Two', 'One', 'Three', 'Four', 'Five'),
    (54, 3, 1, 'Which of the following is a famous quote by Edvard Munch?', '"I was born with a pencil in my hand."', '"I paint not what I see, but what I saw."', '"Nature is not only all that is visible to the eye."', '"From my rotting body, flowers shall grow."', '"Art is the lie that enables us to realize the truth."'),
    (55, 3, 1, 'Which symbolist movement was Edvard Munch associated with?', 'The Symbolist Movement', 'The Impressionist Movement', 'The Surrealist Movement', 'The Cubist Movement', 'The Dada Movement'),
    (56, 3, 1, 'What is the name of the museum where the famous painting "The Scream" is housed?', 'Oslo Munch Museum', 'Louvre Museum', 'Rijksmuseum', 'National Gallery, London', 'Uffizi Gallery'),
    (57, 3, 1, 'Which famous painting by Edvard Munch depicts a couple embracing under a dark, melancholic sky?', 'The Kiss', 'Madonna', 'Puberty', 'The Sick Child', 'Vampire'),
    (58, 3, 1, 'Which of the following is NOT a title of an artwork by Edvard Munch?', 'Starry Night', 'Melancholy', 'Jealousy', 'Anxiety', 'Despair'),
    (59, 3, 1, 'Where was Edvard Munch born?', 'Oslo, Norway', 'Copenhagen, Denmark', 'Stockholm, Sweden', 'Helsinki, Finland', 'Reykjavik, Iceland'),
    (60, 3, 1, 'What was the nationality of Edvard Munch?', 'Norwegian', 'Swedish', 'Danish', 'German', 'Finnish');

