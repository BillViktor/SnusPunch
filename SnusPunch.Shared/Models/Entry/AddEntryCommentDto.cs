﻿namespace SnusPunch.Shared.Models.Entry
{
    public class AddEntryCommentDto
    {
        public int EntryModelId { get; set; }
        public string Comment { get; set; } = "";
        public int? ParentId { get; set; } = null;
        public string? SnusPunchUserNameRepliedTo { get; set; } = null;
    }
}
