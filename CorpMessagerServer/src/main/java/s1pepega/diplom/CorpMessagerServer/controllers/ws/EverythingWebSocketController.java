package s1pepega.diplom.CorpMessagerServer.controllers.ws;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.messaging.handler.annotation.*;
import org.springframework.messaging.simp.SimpMessagingTemplate;
import org.springframework.stereotype.Controller;
import s1pepega.diplom.CorpMessagerServer.entities.Channel;
import s1pepega.diplom.CorpMessagerServer.entities.Message;
import s1pepega.diplom.CorpMessagerServer.entities.UserChannelLink;
import s1pepega.diplom.CorpMessagerServer.exceptions.IllegalSessionIdException;
import s1pepega.diplom.CorpMessagerServer.models.ExceptionResponse;
import s1pepega.diplom.CorpMessagerServer.services.interfaces.ChannelService;
import s1pepega.diplom.CorpMessagerServer.services.interfaces.MessageService;
import s1pepega.diplom.CorpMessagerServer.services.interfaces.SessionService;
import s1pepega.diplom.CorpMessagerServer.services.interfaces.UserChannelService;

@Controller
public class EverythingWebSocketController {
    @Autowired
    private UserChannelService ucService;
    @Autowired
    private ChannelService channelService;
    @Autowired
    private MessageService messageService;
    @Autowired
    private SessionService sessionService;


    @Autowired
    private SimpMessagingTemplate simpMessagingTemplate;

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
    ) throws Exception {
        ucService.delete(link);
    }

    //CHANNEL
    @MessageMapping("/delete_channel/{id}")
    @SendTo("/state/channel/{id}/delete")
    public Channel deleteChannel(
            @DestinationVariable Integer id,
            @Payload Channel channel,
            @Header("sessionId") Integer sessionId
    ) throws Exception {
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
    ) throws Exception {
        return messageService.sendMessage(message);
    }

    @MessageMapping("/channel/{channelId}/change_message")
    @SendTo("/state/channel/{channelId}/msg/change")
    public Message changeMessage(
            @DestinationVariable Integer channelId,
            @Payload Message message,
            @Header("sessionId") Integer sessionId
    ) throws Exception {
        return messageService.editMessage(message);
    }

    @MessageMapping("/channel/{channelId}/delete_message")
    @SendTo("/state/channel/{channelId}/msg/delete")
    public Message deleteMessage(
            @DestinationVariable Integer channelId,
            @Payload Message message,
            @Header("sessionId") Integer sessionId
    ) throws Exception {
        messageService.delete(message.getId());
        return message;
    }

    @MessageMapping("/debug/excTest")
    public Integer testMessage(@Payload Integer sessionId){
        throw new IllegalSessionIdException("TEST MESSAGE EXCEPTION HANDLER",sessionId);
    }

    @MessageExceptionHandler
    public void handleException(Throwable exception){
        if(exception instanceof IllegalSessionIdException){
            try{
                Integer userId = sessionService.getUserIdAsSession(((IllegalSessionIdException) exception).sessionId);
                simpMessagingTemplate.convertAndSend("/state/user/"+userId+"/exception", exception.getMessage());
            }catch (Exception ignored){}
        }
    }

}
