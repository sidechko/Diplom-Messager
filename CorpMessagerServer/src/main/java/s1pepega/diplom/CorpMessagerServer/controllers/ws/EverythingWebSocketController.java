package s1pepega.diplom.CorpMessagerServer.controllers.ws;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.messaging.handler.annotation.*;
import org.springframework.stereotype.Controller;
import s1pepega.diplom.CorpMessagerServer.entities.Channel;
import s1pepega.diplom.CorpMessagerServer.entities.Message;
import s1pepega.diplom.CorpMessagerServer.entities.UserChannelLink;
import s1pepega.diplom.CorpMessagerServer.services.interfaces.ChannelService;
import s1pepega.diplom.CorpMessagerServer.services.interfaces.MessageService;
import s1pepega.diplom.CorpMessagerServer.services.interfaces.UserChannelService;

import java.util.List;

@Controller
public class EverythingWebSocketController {
    @Autowired
    private UserChannelService ucService;
    @Autowired
    private ChannelService channelService;
    @Autowired
    private MessageService messageService;

    //USER
    @MessageMapping("/add_user_to_channel/{userId}")
    @SendTo("/state/user/{userId}/append_to_channel")
    public UserChannelLink addUserToChannel(
            @DestinationVariable Integer userId,
            @Payload UserChannelLink link,
            @Header("sessionId") Integer sessionId
    ){
        return ucService.create(link);
    }

    @MessageMapping("/delete_user_from_channel/{userId}")
    @SendTo("/state/user/{userId}/remove_from_channel")
    public void deleteUserFromChannel(
            @DestinationVariable Integer userId,
            @Payload UserChannelLink link,
            @Header("sessionId") Integer sessionId
    ){
        ucService.delete(link);
    }

    //CHANNEL
    @MessageMapping("/delete_channel/{id}")
    @SendTo("/state/channel/{id}/delete")
    public Channel deleteChannel(
            @DestinationVariable Integer id,
            @Payload Channel channel,
            @Header("sessionId") Integer sessionId
    ){
        channelService.delete(channel.getId());
        ucService.getChannelUsersLink(channel.getId()).forEach(ucService::delete);
        return channel;
    }

    //MESSAGE
    @MessageMapping("/channel/{channelId}/send_message")
    @SendTo("/state/channel/{channelId}/msg/send")
    public Message sendMessage(
            @DestinationVariable Integer channelId,
            @Payload Message message,
            @Header("sessionId") Integer sessionId
    ){
        return messageService.sendMessage(message);
    }

    @MessageMapping("/channel/{channelId}/change_message")
    @SendTo("/state/channel/{channelId}/msg/change")
    public Message changeMessage(
            @DestinationVariable Integer channelId,
            @Payload Message message,
            @Header("sessionId") Integer sessionId
    ){
        return messageService.editMessage(message);
    }

    @MessageMapping("/channel/{channelId}/delete_message")
    @SendTo("/state/channel/{channelId}/msg/delete")
    public Message deleteMessage(
            @DestinationVariable Integer channelId,
            @Payload Message message,
            @Header("sessionId") Integer sessionId
    ){
        messageService.delete(message.getId());
        return message;
    }
}
