using MovieRadar.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MovieRadar.Domain.Interfaces;

namespace MovieRadar.Application.Services
{
    public class CommentService : IService<Comment>
    {
        private readonly ICommentRepository commentRepository;
        public CommentService(ICommentRepository commentRepository)
        {
            this.commentRepository = commentRepository;
        }

        public async Task<IEnumerable<Comment>> GetAll()
        {
            try
            {
                return await commentRepository.GetAll();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while getting all comments: {ex.Message}, inner: {ex.InnerException}");
            }
        }

        public async Task<Comment> GetById(int id)
        {
            try
            {
                return await commentRepository.GetById(id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting comment by id: {ex.Message}, inner: {ex.InnerException}");
            }
        }

        public async Task<int> Add(Comment comment)
        {
            try
            {
                return await commentRepository.Add(comment);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error adding new comment: {ex.Message}, inner: {ex.InnerException}");
            }
        }

        public async Task<bool> Update(Comment comment)
        {
            try
            {
                return await commentRepository.Update(comment);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating comment: {ex.Message}, inner: {ex.InnerException}");
            }
        }

        public async Task<bool> DeleteById(int id)
        {
            try
            {
                return await commentRepository.Delete(id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting comment: {ex.Message}, inner: {ex.InnerException}");
            }
        }
    }
}
