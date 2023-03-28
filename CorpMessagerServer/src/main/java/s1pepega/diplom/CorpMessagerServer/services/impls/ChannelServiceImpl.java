package s1pepega.diplom.CorpMessagerServer.services.impls;

import jakarta.persistence.EntityNotFoundException;
import lombok.RequiredArgsConstructor;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.lang.NonNull;
import org.springframework.stereotype.Service;
import s1pepega.diplom.CorpMessagerServer.entities.Channel;
import s1pepega.diplom.CorpMessagerServer.repositories.ChannelRepository;
import s1pepega.diplom.CorpMessagerServer.services.interfaces.ChannelService;

import java.util.ArrayList;
import java.util.List;
import java.util.stream.Collectors;

import static java.util.Optional.ofNullable;

@Service("ChannelServiceImpl")
@RequiredArgsConstructor
public class ChannelServiceImpl implements ChannelService {
    @Autowired
    private ChannelRepository channelRepository;
    @Override
    public List<Channel> findAll() {
        return new ArrayList<>(channelRepository.findAll());
    }

    @Override
    public Channel findById(Integer id) {
        return channelRepository.findById(id)
                .orElseThrow(()->new EntityNotFoundException("Channel with id "+id+" not found"));
    }

    @Override
    public Channel create(Channel channelResponse) {
        Channel channel = new Channel().setName(channelResponse.getName())
                .setDesc(channelResponse.getDesc());
        return channelRepository.save(channel);
    }

    @Override
    public Channel update(Channel channelRequest) {
        Channel channel = channelRepository.findById(channelRequest.getId())
                .orElseThrow(()->new EntityNotFoundException("Channel with id "+channelRequest.getId()+" not found"));
        ofNullable(channelRequest.getName()).map(channel::setName);
        ofNullable(channelRequest.getDesc()).map(channel::setDesc);
        return channelRepository.save(channel);
    }

    @Override
    public void delete(Integer id) {
        channelRepository.deleteById(id);
    }
}
