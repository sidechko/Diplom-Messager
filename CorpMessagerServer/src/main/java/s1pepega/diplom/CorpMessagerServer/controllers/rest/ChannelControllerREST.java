package s1pepega.diplom.CorpMessagerServer.controllers.rest;

import lombok.RequiredArgsConstructor;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.*;
import s1pepega.diplom.CorpMessagerServer.entities.Channel;
import s1pepega.diplom.CorpMessagerServer.entities.Message;
import s1pepega.diplom.CorpMessagerServer.entities.User;
import s1pepega.diplom.CorpMessagerServer.services.interfaces.ChannelService;
import s1pepega.diplom.CorpMessagerServer.services.interfaces.MessageService;
import s1pepega.diplom.CorpMessagerServer.services.interfaces.UserChannelService;

import java.util.List;

import static org.springframework.util.MimeTypeUtils.APPLICATION_JSON_VALUE;

@RestController
@RequestMapping("/api/channels")
@RequiredArgsConstructor
public class ChannelControllerREST {
    @Autowired
    private ChannelService channelService;
    @Autowired
    private UserChannelService ucService;

    @Autowired
    private MessageService messageService;

    @GetMapping(produces = APPLICATION_JSON_VALUE)
    public List<Channel> findAll() {
        return channelService.findAll();
    }

    @GetMapping(value = "/{channelId}", produces = APPLICATION_JSON_VALUE)
    public Channel findById(@PathVariable Integer channelId) {
        return channelService.findById(channelId);
    }

    @GetMapping(value = "/{channelId}/getUsers", produces = APPLICATION_JSON_VALUE)
    public List<User> getUsers(@PathVariable Integer channelId) {
        return ucService.getChannelUsers(channelId);
    }

    @GetMapping(value = "/{channelId}/getMessagesAll", produces = APPLICATION_JSON_VALUE)
    public List<Message> getMessages(@PathVariable Integer channelId) {
        Channel channel = channelService.findById(channelId);
        return messageService.getMessagesAllInChannel(channel);
    }

    @GetMapping(value = "/{channelId}/getMessages/from{skip}/to{count}", produces = APPLICATION_JSON_VALUE)
    public List<Message> getMessagesFromTo(
            @PathVariable Integer channelId,
            @PathVariable Integer skip,
            @PathVariable Integer count) {
        Channel channel = channelService.findById(channelId);
        return messageService.getMessagesInChannel(channel,skip,count);
    }

    @PostMapping(value = "/create", consumes = APPLICATION_JSON_VALUE, produces = APPLICATION_JSON_VALUE)
    public Channel create(@RequestBody Channel request) {
        return channelService.create(request);
    }

    @PatchMapping(value = "/update", consumes = APPLICATION_JSON_VALUE, produces = APPLICATION_JSON_VALUE)
    public Channel update(@RequestBody Channel request) {
        return channelService.update(request);
    }

    @DeleteMapping(value = "/delete/{id}", consumes = APPLICATION_JSON_VALUE, produces = APPLICATION_JSON_VALUE)
    public Channel delete(@PathVariable Integer id) {
        Channel channel = channelService.findById(id);
        channelService.delete(id);
        return channel;
    }
}
