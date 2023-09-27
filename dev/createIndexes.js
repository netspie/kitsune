function createIndexes() {
	db.events.createIndex({ Timestamp: 1 })
	db.rehearseItems.createIndex({ Owner: 1, RepsInterval: 1, ItemType: 1, Mode: 1 })
	db.rehearseItemsAsap.createIndex({ Owner: 1 })
	db.rehearseEntities.createIndex({ Owner: 1, IsItem: 1 })
}