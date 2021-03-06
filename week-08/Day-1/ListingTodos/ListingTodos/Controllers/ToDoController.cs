﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ListingTodos.Models;
using ListingTodos.Repositories;
using ListingTodos.ViewModels;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ListingTodos.Controllers
{
    [Route("")]
    public class ToDoController : Controller
    {
        public TodoRepository todoRepository;
        public TodoViewModel todoViewModel;

        public ToDoController(TodoRepository todoRepository, TodoViewModel todoViewModel)
        {
            this.todoRepository = todoRepository;
            this.todoViewModel = todoViewModel;
        }

        //[Route("todo")]
        //public IActionResult FilterToDo([FromQuery] bool isActive)
        //{
        //    //http://localhost:53454/todo/?isActive=true
        //    var ActiveToDos = todoRepository.ListOfToDos().Where(t => t.IsDone != isActive).ToList();
        //    return View("List", ActiveToDos);
        //}

        [Route("")]
        [HttpGet("list")]
        public IActionResult List()
        {
            todoViewModel.userList = todoRepository.ListOfToUsers();
            todoViewModel.todoList = todoRepository.ListOfToDos();
            return View(todoViewModel);
        }

        //[Route("add")]
        //public IActionResult AddToDo([FromQuery] string title, [FromQuery] bool isDone)
        //{
        //    //http://localhost:53454/add/?title=swim&isDone=true
        //    ToDo newToDo = new ToDo()
        //    {
        //        Title = title,
        //        IsDone = isDone
        //    };

        //    todoRepository.AddNewTodo(newToDo);
        //    return RedirectToAction("list");
        //}

        [HttpPost("add")]
        public IActionResult Add(string title, string username, string assigneename)
        {
            //string name = Request.Form["username"];
            var currentAssignee = todoRepository.toDoContext.Users.FirstOrDefault(x => x.Name == assigneename);
            var currentCreator = todoRepository.toDoContext.Users.FirstOrDefault(x => x.Name == username);
            ToDo newToDo = new ToDo()
            {
                Title = title,
                Creator = currentCreator,
                Assignee = currentAssignee
            };

            todoRepository.AddNewTodo(newToDo);
            return RedirectToAction("list");
        }

        [HttpGet("add")]
        public IActionResult Add()
        {
            return View(todoRepository.ListOfToUsers());
        }

        [HttpGet("delete/{id}")]
        public IActionResult Delete(long id)
        {
            todoRepository.DeleteToDo(id);
            return RedirectToAction("list");
        }

        [HttpPost("edit/{id}")]
        public IActionResult Edit(ToDo toDo, long id, long assigneeid)
        {
            var currentAssignee = todoRepository.toDoContext.Users.FirstOrDefault(x => x.UserId == assigneeid);
            toDo.Assignee = currentAssignee;
            todoRepository.Edit(toDo, id);
            return RedirectToAction("list");
        }

        [HttpGet("edit/{id}")]
        public IActionResult Edit(long id)
        {
            //var currentTodo = todoRepository.toDoContext.ToDos.FirstOrDefault(x => x.Id == id);
            todoViewModel.userList = todoRepository.ListOfToUsers();
            todoViewModel.todoList = todoRepository.ListOfToDos();
            todoViewModel.IdOfTodo = id;
            return View(todoViewModel);
        }

        [HttpGet("{id}")]
        public IActionResult Todo(long id)
        {
            //var currentTodo = todoRepository.toDoContext.ToDos.FirstOrDefault(x => x.Id == id);
            todoViewModel.userList = todoRepository.ListOfToUsers();
            todoViewModel.todoList = todoRepository.ListOfToDos();
            todoViewModel.IdOfTodo = id;
            return View(todoViewModel);
        }

        [HttpGet("userlist")]
        public IActionResult UserList()
        {
            return View(todoRepository.ListOfToUsers());
        }

        [HttpPost("adduser")]
        public IActionResult AddUser(string name)
        {
            User newUser = new User()
            {
                Name = name,
            };

            todoRepository.AddNewUser(newUser);
            return RedirectToAction("userlist");
        }

        [HttpGet("adduser")]
        public IActionResult AddUser()
        {
            return View();
        }
    }
}
