package s1pepega.diplom.CorpMessagerServer.services.impls;

import lombok.RequiredArgsConstructor;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;
import s1pepega.diplom.CorpMessagerServer.entities.Channel;
import s1pepega.diplom.CorpMessagerServer.entities.User;
import s1pepega.diplom.CorpMessagerServer.entities.UserChannelLink;
import s1pepega.diplom.CorpMessagerServer.repositories.UserChannelRepository;
import s1pepega.diplom.CorpMessagerServer.services.interfaces.UserChannelService;

import java.util.List;
import java.util.stream.Collectors;

@Service("UserChannelServiceImpl")
@RequiredArgsConstructor
public class UserChannelServiceImpl implements UserChannelService {

    @Autowired
    private UserChannelRepository ucRepository;

    @Override
    @Transactional(readOnly = true)
    public List<UserChannelLink> findAll() {
        return ucRepository.findAll();
    }

    @Override
    @Transactional(readOnly = true)
    public List<Channel> getUserChannels(Integer userId) {
        return ucRepository.getUserChannels(userId)
                .stream()
                .map(UserChannelLink::getChannel)
                .collect(Collectors.toList());
    }

    @Override
    @Transactional(readOnly = true)
    public List<UserChannelLink> getUserChannelsLink(Integer userId) {
        return ucRepository.getUserChannels(userId);
    }

    @Override
    @Transactional(readOnly = true)
    public List<User> getChannelUsers(Integer channelId) {
        return ucRepository.getChannelUsers(channelId)
                .stream()
                .map(UserChannelLink::getUser)
                .collect(Collectors.toList());
    }

    @Override
    @Transactional(readOnly = true)
    public List<UserChannelLink> getChannelUsersLink(Integer channelId) {
        return ucRepository.getChannelUsers(channelId);
    }

    @Override
    @Transactional
    public UserChannelLink create(UserChannelLink link) {
        return ucRepository.save(link);
    }

    @Override
    @Transactional
    public void delete(UserChannelLink link) {
        ucRepository.delete(link);
    }
}
