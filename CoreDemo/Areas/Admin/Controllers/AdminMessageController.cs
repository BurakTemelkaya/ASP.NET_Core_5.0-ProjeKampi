using AutoMapper;
using BusinessLayer.Abstract;
using EntityLayer.Concrete;
using EntityLayer.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreDemo.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Moderator")]
    public class AdminMessageController : Controller
    {
        private readonly IMessageService _messageService;
        private readonly IMessageDraftService _messageDraftService;
        private readonly IUserBusinessService _userService;
        private readonly IMapper _mapper;

        public AdminMessageController(IMessageService messageService, IUserBusinessService userService, IMessageDraftService messageDraftService,
            IMapper mapper)
        {
            _messageService = messageService;
            _userService = userService;
            _messageDraftService = messageDraftService;
            _mapper = mapper;
        }

        public IActionResult Inbox()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetInboxMessages(string search)
        {
            var values = await _messageService.GetInboxWithMessageListAsync(User.Identity.Name, search);
            if (values.Success)
            {
                var jsonValues = JsonConvert.SerializeObject(values.Data);
                return Json(jsonValues);
            }
            return BadRequest(values.Message);
        }

        public async Task<IActionResult> SendBox(string search = null)
        {
            List<MessageReceiverUserDto> values = null;
            if (search != null)
            {
                var result = await _messageService.GetSendBoxWithMessageListAsync(User.Identity.Name,
                x => x.Subject.ToLower().Contains(search.ToLower()));

                values = result.Data;

            }
            if (values == null || values.Any())
            {
                var result = await _messageService.GetSendBoxWithMessageListAsync(User.Identity.Name);
                values = result.Data;
            }
            return View(values.ToList());
        }

        [HttpGet]
        public async Task<IActionResult> SendMessage(int id, string ReceiverUser = null)
        {
            if (id != 0)
            {
                var value = await _messageDraftService.GetByIDAsync(id, User.Identity.Name);
                if (value != null)
                {
                    var message = _mapper.Map<Message>(value.Data);
                    return View(message);
                }
            }
            if (ReceiverUser != null)
            {
                ViewBag.ReceiverUser = ReceiverUser;
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SendMessage(Message message, string receiver)
        {
            var result = await _messageService.AddMessageAsync(message, User.Identity.Name, receiver);
            if (result.Success)
            {
                return RedirectToAction("SendBox");
            }

            ModelState.AddModelError("", result.Message);
            return View(message);
        }
        public async Task<IActionResult> Read(int id)
        {
            var value = _messageService.GetReceivedMessageAsync(User.Identity.Name, x => x.MessageID == id).Result.Data;

            if (value == null)
                value = _messageService.GetSendMessageAsync(User.Identity.Name, x => x.MessageID == id).Result.Data;

            if (value == null)
                return RedirectToAction("Inbox");

            await _messageService.MarkUsReadAsync(id, User.Identity.Name);
            return View(value);
        }
        public async Task<IActionResult> Delete(int id)
        {
            await _messageService.DeleteMessageAsync(id, User.Identity.Name);
            return RedirectToAction("Inbox");
        }

        [HttpPost]
        public async Task<IActionResult> MarkReadMessages(List<string> selectedItems)
        {
            var result = await _messageService.MarksUsReadAsync(selectedItems, User.Identity.Name);

            if (result.Success)
            {
                return Ok();
            }

            return BadRequest(result.Message);
        }

        [HttpPost]
        public async Task<IActionResult> MarkUnreadMessages(List<string> selectedItems)
        {
            var result = await _messageService.MarksUsUnreadAsync(selectedItems, User.Identity.Name);

            if (result.Success)
            {
                return Ok();
            }

            return BadRequest(result.Message);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteMessages(List<string> selectedItems)
        {
            var result = await _messageService.DeleteMessagesAsync(selectedItems, User.Identity.Name);

            if (result.Success)
            {
                return Ok();
            }

            return BadRequest(result.Message);
        }

        [HttpGet]
        public async Task<IActionResult> GetUnreadMessagesCount()
        {
            var value = await _messageService.GetUnreadMessagesCountByUserNameAsync(User.Identity.Name);
            return Json(value.Data);
        }

        [HttpGet]
        public async Task<IActionResult> GetDraftMessagesCount()
        {
            var value = await _messageDraftService.GetCountByUserNameAsync(User.Identity.Name);
            return Json(value.Data);
        }
    }
}
