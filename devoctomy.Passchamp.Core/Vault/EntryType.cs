namespace devoctomy.Passchamp.Core.Vault;

public enum EntryType
{
    None = 0,
    CreatedVault = 1,
    CreatedCredential = 2,
    ModifyName = 3,
    ModifyDescription = 4,
    AddCredential = 5,
    ModifyGlyphKey = 6,
    ModifyGlyphColour = 7,
    ModifyNotes = 8,
    ModifyUsername = 9,
    ModifyPassword = 10,
    ModifyTags = 11,
    RemovedCredential = 12,
    Saved = 13
}
