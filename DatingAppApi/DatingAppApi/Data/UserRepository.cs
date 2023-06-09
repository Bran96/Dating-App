﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using Azure;
using DatingAppApi.DTO_s;
using DatingAppApi.Entities;
using DatingAppApi.Helpers;
using DatingAppApi.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DatingAppApi.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public UserRepository(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<MemberDto> GetMemberAsync(string username) // Now we extending the UserRepo, just to return "some" of the fields of the table. And now we need to make use of AutoMapper for MemberDto to return some of the data
        {
            // This is gonna give us a single MemberDto according to specific username
            // Instead of doing the Mapping in the controller, we rather doing it in the Repository
            return await _context.Users.Where(x => x.UserName == username).ProjectTo<MemberDto>(_mapper.ConfigurationProvider).SingleOrDefaultAsync();
        }

        // The MemberDto is what we return in our result, basically what we're getting back
        public async Task<PagedList<MemberDto>> GetMembersAsync(UserParams userParams) // Now we extending the UserRepo, just to return "some" of the fields of the table. And now we need to make use of AutoMapper for MemberDto to return some of the data
        {
            var query = _context.Users.AsQueryable(); //AsQueryable allows you to perform more advanced querying operations using the LINQ query operators.

            query = query.Where(u => u.UserName != userParams.CurrentUserName); // Get all the users except for the Current logged in user
            query = query.Where(u => u.Gender == userParams.Gender); // Get all the users according to the gender selected

            // Filtering according to date Range from Minimum age to maximum age
            // Also keep in mind in our database we strore it as a dob value and it has a date property
            // and in the userParams we store it as 2 properties of datatype "int", so we need to add logic to 2 variables
            var minDob = DateTime.Today.AddYears(-userParams.MaxAge - 1);
            var maxDob = DateTime.Today.AddYears(-userParams.MinAge);
            query = query.Where(u => u.DateOfBirth >= minDob && u.DateOfBirth <= maxDob);

            //This is for sorting the users or Order the users
            query = userParams.OrderBy switch
            {
                "created" => query.OrderByDescending(u => u.Created),
                _ => query.OrderByDescending(u => u.LastActive)// Specify just the default which is the "lastActive"
            };

            // This CreateAsync method is inside
            return await PagedList<MemberDto>.CreateAsync(query.AsNoTracking().ProjectTo<MemberDto>(_mapper.ConfigurationProvider), userParams.PageNumber, userParams.PageSize);
        }

        public async Task<AppUser> GetUserByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<AppUser> GetUserByUsernameAsync(string username)
        {
            // Can either use FirstOrDefaultAsync or SingleOrDefaultAsync
            // In this method we need to use "Include" specifically to include the photos, but with ProjectTo in the GetMemberAsync method it handles everything already with regards to the relationship with Photo
            // .Include in this case means we eagerloading, but ProjectTo handles that for us which make code less
            return await _context.Users.Include(p => p.Photos).SingleOrDefaultAsync(x => x.UserName == username); // This is basically returning all the fields from the table
        }

        public async Task<IEnumerable<AppUser>> GetUsersAsync()
        {
            // In this method we need to use "Include" specifically to include the photos, but with ProjectTo in the GetMemberAsync method it handles everything already with regards to the relationship with Photo
            // .Include in this case means we eagerloading, but ProjectTo handles that for us which make code less
            return await _context.Users.Include(p => p.Photos).ToListAsync(); // This is basically returning all the fields from the table
        }

        public async Task<bool> SaveAllAsync()
        {
            // This method is going to return how many changes were made to the database, so in order to retur a boolean we want to make sure that the changes are greater than 0
            return await _context.SaveChangesAsync() > 0; // If its 0 it will return false
        }

        public void Update(AppUser user)
        {
            // We are not returning anything from this
            // We are not saving anything from this method, we just informing that the entity has been updated
            _context.Entry(user).State = EntityState.Modified; // This just tells ouur EntityFramework tracker that something has chnaged with the entity(AppUser) that we have passed in the parentheses
        }
    }
}
