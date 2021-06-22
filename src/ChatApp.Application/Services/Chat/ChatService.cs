using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ChatApp.Application.Services.WebsocketSender;
using ChatApp.Domain.Models;
using ChatApp.Domain.Repositories;

namespace ChatApp.Application.Services.Chat
{
    public class ChatService : IChatService
    {
        private readonly ILobbyRepo _lobbyRepo;
        private readonly IRoomRepo _roomRepo;
        private readonly IAnonymousUserRepo _anonymousUserRepo;
        private readonly IWebsocketSenderService _websocketSenderService;
        public ChatService(ILobbyRepo lobbyRepo, IRoomRepo roomRepo, IAnonymousUserRepo anonymousUserRepo, IWebsocketSenderService websocketSenderService)
        {
            _lobbyRepo = lobbyRepo;
            _roomRepo = roomRepo;
            _anonymousUserRepo = anonymousUserRepo;
            _websocketSenderService = websocketSenderService;
        }

        public async Task JoinToLobbyAsync(AnonymousUser anonymousUser)
        {
            var lobby = await _lobbyRepo.GetLobby();
            lobby.Add(anonymousUser);
            var pairedUsers = lobby.DequeuePairedUsers();
            await _lobbyRepo.UpdateLobby(lobby);
            await _websocketSenderService.SendMessage(anonymousUser, "We looking for person for you...");
            if(pairedUsers == null) return;
            var room = new Room(pairedUsers);
            await _roomRepo.AddRoom(room);
            await _websocketSenderService.StartChatting(room);
        }

        public async Task SendMessageToRoomAsync(AnonymousUser anonymousUser, string message)
        {
            if(string.IsNullOrWhiteSpace(message)) return;
            if(anonymousUser == null) throw new NullReferenceException("Anonymous user cannot be null.");
            var room = await _roomRepo.GetRoom(anonymousUser);
            await _websocketSenderService.SendMessageToRoom(room, message);
        }

        public async Task StopChattingAsync(AnonymousUser anonymousUser)
        {
            
            var room = await _roomRepo.GetRoom(anonymousUser);
            await _websocketSenderService.StopChatting(room);


            room.Emptting();

            // unitOfWork implementation needed
            foreach(var user in room.AnonymousUsers) await _anonymousUserRepo.UpdateAsync(user);
            await _roomRepo.DeleteAsync(room);      
        }
    }
}