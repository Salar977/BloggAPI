﻿using Blogg.Models.Entities;

namespace Blogg.Repository.interfaces;

public interface ICommentRepository : IRepository<Comment>
{
    Task<Comment?> AddAsync(Comment comment);
    Task<ICollection<Comment>> GetCommentsForPostAsync(int PostId);
}