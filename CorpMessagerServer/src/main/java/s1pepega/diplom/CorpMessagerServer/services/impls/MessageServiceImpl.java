package s1pepega.diplom.CorpMessagerServer.services.impls;

import jakarta.persistence.EntityNotFoundException;
import lombok.RequiredArgsConstructor;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;
import s1pepega.diplom.CorpMessagerServer.entities.Channel;
import s1pepega.diplom.CorpMessagerServer.entities.Message;
import s1pepega.diplom.CorpMessagerServer.repositories.ChannelRepository;
import s1pepega.diplom.CorpMessagerServer.repositories.MessageRepository;
import s1pepega.diplom.CorpMessagerServer.repositories.UserRepository;
import s1pepega.diplom.CorpMessagerServer.services.interfaces.MessageService;

import java.util.ArrayList;
import java.util.Date;
import java.util.List;

import static java.util.Optional.ofNullable;

@Service("MessageServiceImpl")
@RequiredArgsConstructor
public class MessageServiceImpl implements MessageService {
    @Autowired
    private MessageRepository messageRepository;
    @Autowired
    private UserRepository userRepository;
    @Autowired
    private ChannelRepository channelRepository;

    @Override
    @Transactional(readOnly = true)
    public List<Message> findAll() {
        return new ArrayList<>(messageRepository.findAll());
    }

    @Override
    @Transactional(readOnly = true)
    public Message findById(Integer id) {
        return messageRepository.findById(id)
                .orElseThrow(() -> new EntityNotFoundException("Message with id " + id + " not found"));
    }

    @Override
    @Transactional(readOnly = true)
    public List<Message> getMessagesInChannel(Channel channel, Integer start, Integer count) {
        return new ArrayList<>(messageRepository.getMessageInChannelWithStartAndCount(
                channel.getId(),
                count,
                start
        ));
    }

    @Override
    @Transactional(readOnly = true)
    public List<Message> getMessagesAllInChannel(Channel channel) {
        return new ArrayList<>(messageRepository.getMessageAllInChannel(channel.getId()));
    }

    @Override
    @Transactional
    public Message editMessage(Message messageRequest) {
        Message message = messageRepository.findById(messageRequest.getId())
                .orElseThrow(() -> new EntityNotFoundException("Message with id " + messageRequest.getId() + " not found"));
        ofNullable(messageRequest.getContent()).map(message::setContent);
        message.setUpdateTime(new Date());
        return messageRepository.save(message);
    }

    @Override
    @Transactional
    public Message sendMessage(Message sendMessageByUserRequest) {
        userRepository.findById(sendMessageByUserRequest.getSender().getId())
                .orElseThrow(() -> new EntityNotFoundException("User with id "+sendMessageByUserRequest.getSender().getId()+" not found"));
        channelRepository.findById(sendMessageByUserRequest.getChannel().getId())
                .orElseThrow(()-> new EntityNotFoundException("Channel with id "+sendMessageByUserRequest.getChannel().getId()+" not found"));
        sendMessageByUserRequest.setSendTime(new Date());
        return messageRepository.save(sendMessageByUserRequest);
    }

    @Override
    @Transactional
    public void delete(Integer id) {
        messageRepository.deleteById(id);
    }
}
